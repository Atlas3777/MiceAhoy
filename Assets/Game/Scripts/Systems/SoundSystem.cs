using Game.Scripts.Aspects;
using Leopotam.EcsProto;
using Leopotam.EcsProto.QoL;
using UnityEngine;

namespace Game.Scripts.Systems
{
    public enum SoundType
    {
        AngryGuest,
        CookingComplete,
        ReputationLoss,
        StoveSound,
        Pick,
        Place,
        Whoa,
    }

    public class SoundSystem : IProtoInitSystem, IProtoRunSystem
    {
        private readonly GameResources _gameResources;
        private readonly SoundManager _soundManager;
        [DI] private readonly ProtoWorld _world;
        [DI] private readonly BaseAspect _baseAspect;

        private ProtoIt _startIt;
        private ProtoIt _stopIt;
        private ProtoIt _sfxIt;

        public SoundSystem(GameResources gameResources, SoundManager soundManager)
        {
            _gameResources = gameResources;
            _soundManager = soundManager;
        }

        public void Init(IProtoSystems systems)
        {
            _sfxIt = new(new[] { typeof(PlaySFXRequest) });
            _sfxIt.Init(_world);

            _startIt = new(new[] { typeof(StartLoopSound) });
            _startIt.Init(_world);

            _stopIt = new(new[] { typeof(StopLoopSound), typeof(ActiveLoopSound) });
            _stopIt.Init(_world);
        }

        public void Run()
        {
            foreach (var sfxEntity in _sfxIt)
            {
                ref var req = ref sfxEntity.Get<PlaySFXRequest>();
                var clip = GetClip(req.SoundType);
                if (clip != null)
                {
                    _soundManager.PlaySfx(clip);
                }

                sfxEntity.Del<PlaySFXRequest>(); 
            }

            foreach (var entity in _startIt)
            {
                ref var startData = ref entity.Get<StartLoopSound>();
                var clip = GetClip(startData.SoundType);

                if (clip != null)
                {
                    var guid = _soundManager.PlayLoopingSfx(clip);
        
                    ref var active = ref entity.GetOrAdd<ActiveLoopSound>();
                    active.InternalId = guid;
                    active.SoundType = startData.SoundType;
                }

                entity.Del<StartLoopSound>();
            }

            foreach (var stopEntity in _stopIt)
            {
                ref var active = ref stopEntity.Get<ActiveLoopSound>();

                _soundManager.StopLoopingSfx(active.InternalId);

                _baseAspect.StopLoopSoundPool.Del(stopEntity);
                _baseAspect.ActiveLoopSoundPool.Del(stopEntity);
            }
        }

        private AudioClip GetClip(SoundType type)
        {
            return type switch
            {
                SoundType.StoveSound => _gameResources.SoundsLink.stove_loop_sfx,
                SoundType.Place =>_gameResources.SoundsLink.pick_place,
                SoundType.Pick =>_gameResources.SoundsLink.pick_place,
                SoundType.AngryGuest =>_gameResources.SoundsLink.angry_guest,
                SoundType.ReputationLoss =>_gameResources.SoundsLink.reputation_loss,
                SoundType.CookingComplete =>_gameResources.SoundsLink.cooking_done,
                SoundType.Whoa => GetWhoaClip()
            };
        }
        private AudioClip GetWhoaClip()
        {
            float chance = Random.value; 
            if (chance < 0.1f) 
            {
                return _gameResources.SoundsLink.whoa2;
            }
            return _gameResources.SoundsLink.whoa;
        }
    }
}