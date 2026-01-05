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
        
        private ProtoIt _hungryGuestsIt;

        public void Init(IProtoSystems systems)
        {
            _hungryGuestsIt = new(new[] { typeof(GuestStateComponent), typeof(WaitingOrderTag) });
            _hungryGuestsIt.Init(systems.World());
        }

        public void Run()
        {
            foreach (var guestEntity in _hungryGuestsIt)
            {
                ref var guestState = ref _guestAspect.GuestStateComponentPool.Get(guestEntity);

                if (guestState.Hunger <= 0)
                {
                    Debug.Log("Гость наелся, уходит");
                    
                    _guestAspect.WaitingOrderTagPool.Del(guestEntity);
                    _guestAspect.GuestServedEventPool.Add(guestEntity);
                    _guestAspect.GuestServicedTagPool.Add(guestEntity);
                    _baseAspect.TimerCompletedPool.Add(guestEntity);
                }
            }
        }
    }
}