using Game.Script.Aspects;
using Game.Scripts.Aspects;
using Game.Scripts.Infrastructure;
using Leopotam.EcsProto;
using Leopotam.EcsProto.QoL;
using UnityEngine;

namespace Game.Scripts.Systems
{
    public class AngryGuestLeaveSystem : IProtoInitSystem, IProtoRunSystem
    {
        [DI] GuestAspect _guestAspect;
        [DI] WorkstationsAspect _workstationsAspect;
        [DI] BaseAspect _baseAspect;
        [DI] ProtoWorld _world;
        private ProtoIt _it;
        private ProtoItExc _occupiedTablesIt;

    
        public void Init(IProtoSystems systems)
        {
            _it = new(new[] { typeof(WaitingOrderTag), typeof(TimerCompletedEvent),  typeof(GuestStateComponent) });
        
            _occupiedTablesIt = new(
                new[] { typeof(GuestTableComponent) },
                new[] { typeof(GuestTableIsFreeTag) });
            _it.Init(_world);
            _occupiedTablesIt.Init(_world);
        }

        public void Run()
        {
            foreach (var guestEntity in _it)
            {
                Debug.LogError("ПРОЕБАЛИ ожидание заказа");

                var guest = guestEntity.Get<GuestStateComponent>();
            
            
                _guestAspect.WaitingOrderTagPool.Del(guestEntity);
                _guestAspect.GuestServedEventPool.GetOrAdd(guestEntity);
                _guestAspect.GuestServicedTagPool.GetOrAdd(guestEntity);
                _guestAspect.GuestIsWalkingTagPool.Add(guestEntity);
            
            
                _world.NewEntityWith<ReputationRequest>().Diff = guest.ReputationLoss;
                _world.NewEntityWith<PlaySFXRequest>().SoundType = SoundType.AngryGuest;
            }

            foreach (var tableEntity in _occupiedTablesIt)
            {
                var guestEntity = _workstationsAspect.GuestTablePool.Get(tableEntity).Guest.GetEntity();

                if (_guestAspect.GuestServicedTagPool.Has(guestEntity))
                {
                    _guestAspect.GuestTableIsFreeTagPool.Add(tableEntity);
                    Debug.Log("СВОБОДНАЯ КАССА");
                }
            }
        }
    }
}