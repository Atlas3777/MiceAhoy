using System.Linq;
using Game.Scripts.Aspects;
using Game.Scripts.Infrastructure;
using Leopotam.EcsProto;
using Leopotam.EcsProto.QoL;
using UnityEngine;

namespace Game.Scripts.Systems
{
    public class LevelDirectorSystem : IProtoInitSystem, IProtoRunSystem
    {
        [DI] private ProtoWorld _world;
        [DI] private BaseAspect _baseAspect;

        private readonly LevelConfig _config;
        private readonly LevelState _levelState;

        private int PlayerCount = 1;

        public LevelDirectorSystem(LevelState levelState, LevelConfig config)
        {
            _config = config;
            _levelState = levelState;
        }

        public void Init(IProtoSystems systems)
        {
            _levelState.LevelDuration = _config.LevelDuration;
            _levelState.AccumulatedCredits += _config.BaseCreditWallet;
            _levelState.NextSpawnTime = Time.time + _config.SpawnIntervalMin;
            _levelState.LastSuccessfulSpawnTime = Time.time;
        }

        public void Run()
        {
            var state = _levelState;

            if (state.ElapsedTime >= state.LevelDuration)
            {
                state.TimedOut = true;
                return;
            }

            state.ElapsedTime += Time.fixedDeltaTime;
            float progress = Mathf.Clamp01(state.ElapsedTime / state.LevelDuration);

            // накопление кредитов: базовая ставка * кривая
            float creditsPerSecond = _config.BaseCreditsPerSecond * _config.DifficultyCurve.Evaluate(progress);
            creditsPerSecond *= (1 + (PlayerCount - 1) * 0.5f);
            state.AccumulatedCredits += creditsPerSecond * Time.fixedDeltaTime;

            // не спавним если достигнут лимит активных гостей
            if (state.ActiveGuest >= _config.MaxConcurrentGuests)
            {
                // попробуем проверить чуть позже
                if (Time.time > state.NextSpawnTime) state.NextSpawnTime = Time.time + _config.MinSpawnRetryInterval;
                return;
            }

            // таймер
            if (Time.time < state.NextSpawnTime) return;

            float currentIntensity = _config.IntensityCurve.Evaluate(progress);

            var selectedGuest = _config.SpawnStrategy?.TrySelectGuest(
                state.AccumulatedCredits,
                _config.AvailableGuests,
                progress,
                currentIntensity,
                state.LastSpawnAtByGuestId,
                Time.time
            );

            float rawInterval = Random.Range(_config.SpawnIntervalMin, _config.SpawnIntervalMax);
            float intensityModifier = Mathf.Lerp(1.5f, 0.5f, currentIntensity);

            if (selectedGuest != null)
            {
                // выполнить спавн
                state.AccumulatedCredits -= selectedGuest.Cost;
                ref var spawnGuestRequest = ref _baseAspect.SpawnGuestRequestPool.NewEntity();
                spawnGuestRequest.Profile = selectedGuest;

                state.ActiveGuest++;
                state.LastSpawnAtByGuestId[selectedGuest.GetInstanceID()] = Time.time;
                state.LastSuccessfulSpawnTime = Time.time;

                state.NextSpawnTime = Time.time + (rawInterval * intensityModifier);
            }
            else
            {
                // стратегия вернула null: увеличить шанс следующей попытки (короче пауза), но не мгновенно
                // если долго не было успешного спавна — форсим самого дешёвого гостя
                float sinceLastSuccess = Time.time - state.LastSuccessfulSpawnTime;
                if (sinceLastSuccess >= _config.ForcedSpawnAfterNoSpawnSeconds)
                {
                    var cheapest = _config.AvailableGuests.OrderBy(g => g.Cost)
                        .FirstOrDefault(g => g.Cost <= state.AccumulatedCredits);
                    if (cheapest != null)
                    {
                        // форс-спавним дешёвого
                        state.AccumulatedCredits -= cheapest.Cost;
                        ref var spawnGuestRequest = ref _baseAspect.SpawnGuestRequestPool.NewEntity();
                        spawnGuestRequest.Profile = cheapest;

                        state.ActiveGuest++;
                        state.LastSpawnAtByGuestId[cheapest.GetInstanceID()] = Time.time;
                        state.LastSuccessfulSpawnTime = Time.time;

                        state.NextSpawnTime = Time.time + (rawInterval * intensityModifier);
                        return;
                    }
                }

                // иначе попробуем повторить чуть раньше
                state.NextSpawnTime = Time.time +
                                      Mathf.Max(_config.MinSpawnRetryInterval, rawInterval * intensityModifier * 0.5f);
            }
        }
    }
}