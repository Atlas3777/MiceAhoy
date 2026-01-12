using System;
using Game.Script.Aspects;
using Game.Scripts.Aspects;
using Leopotam.EcsProto;
using Leopotam.EcsProto.QoL;
using UnityEngine;

namespace Game.Scripts.Systems
{
    public class HappyGuestLeaveSystem : IProtoInitSystem, IProtoRunSystem
    {
        [DI] private GuestAspect _guestAspect;
        [DI] private WorkstationsAspect _workstationsAspect;
        [DI] private BaseAspect _baseAspect;
        [DI] private ProtoWorld _world;
        
        private ProtoIt _hungryGuestsIt;
        
        public event Action OnGuestFed;


        public void Init(IProtoSystems systems)
        {
            _hungryGuestsIt = new(new[] { typeof(GuestStateComponent), typeof(WaitingOrderTag) });
            _hungryGuestsIt.Init(_world);
        }

        public void Run()
        {
            foreach (var guestEntity in _hungryGuestsIt)
            {
                ref var guestState = ref _guestAspect.GuestStateComponentPool.Get(guestEntity);

                if (guestState.Hunger <= 0)
                {
                    Debug.Log("Гость наелся, уходит");

                    _world.NewEntityWith<PlaySFXRequest>().SoundType = SoundType.Whoa; 
                    OnGuestFed?.Invoke();
                    
                    _guestAspect.WaitingOrderTagPool.Del(guestEntity);
                    _guestAspect.GuestServedEventPool.Add(guestEntity);
                    _guestAspect.GuestServicedTagPool.Add(guestEntity);
                    _baseAspect.TimerCompletedPool.Add(guestEntity);
                }
            }
        }
    }
}