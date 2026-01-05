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
        private readonly LevelStateService _levelStateService;

        private int PlayerCount = 1; //#TODO aga

        public LevelDirectorSystem(LevelStateService levelStateService, LevelConfig config)
        {
            _config = config;
            _levelStateService = levelStateService;
        }
        
        public void Init(IProtoSystems systems)
        {
            _levelStateService.LevelDuration = _config.LevelDuration;
        }
        
        public void Run()
        {
            Debug.Log("LevelDirectorSystem Run");
            var director = _levelStateService;
            director.ElapsedTime += Time.deltaTime;

            // 2. Начисляем кредиты
            // Берем значение из кривой сложности в зависимости от прогресса уровня (0..1)
            float progress = director.ElapsedTime / director.LevelDuration;
            float creditsPerSecond = _config.DifficultyCurve.Evaluate(progress);

            // Скейлинг от игроков:
            creditsPerSecond *= (1 + (PlayerCount - 1) * 0.5f); // +50% сложности за доп. игрока

            director.AccumulatedCredits += creditsPerSecond * Time.deltaTime;

            // 3. Проверка таймера спавна (чтобы не спамить каждую секунду)
            if (Time.time < director.NextSpawnTime) return;

            // 4. Попытка купить гостя
            if (TryBuyGuest(director.AccumulatedCredits, out GuestProfile selectedGuest))
            {
                director.AccumulatedCredits -= selectedGuest.Cost;

                // Создаем запрос на спавн (другая система его обработает и создаст View)
                ref var spawnGuestRequest = ref _baseAspect.SpawnGuestRequestPool.NewEntity();
                spawnGuestRequest.Profile = selectedGuest;
                _levelStateService.ActiveGuest++;

                // 5. Рандомизация следующего интервала (Реиграбельность!)
                float interval = Random.Range(_config.SpawnIntervalMin, _config.SpawnIntervalMax);
                director.NextSpawnTime = Time.time + interval;
            }
        }

        private bool TryBuyGuest(float budget, out GuestProfile result)
        {
            result = null;
            // Фильтруем тех, кого можем позволить
            var affordable = _config.AvailableGuests.Where(g => g.Cost <= budget).ToList();

            if (affordable.Count == 0) return false;

            // Взвешенный рандом (чтобы редкие/дорогие враги падали реже, но падали)
            // Или просто Shuffle, если хочешь чистого хаоса
            result = affordable[Random.Range(0, affordable.Count)];
            return true;
        }
    }
}