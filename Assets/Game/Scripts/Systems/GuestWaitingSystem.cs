using Game.Script.Aspects;
using Game.Scripts.Aspects;
using Leopotam.EcsProto;
using Leopotam.EcsProto.QoL;
using UnityEngine;

namespace Game.Script.Systems
{
    public class GuestWaitingSystem : IProtoInitSystem, IProtoRunSystem, IProtoDestroySystem
    {
        [DI] readonly ProtoWorld _world;
        [DI] readonly PlayerAspect _playerAspect;
        [DI] readonly ViewAspect _viewAspect;
        [DI] readonly BaseAspect _baseAspect;
        [DI] readonly GuestAspect _guestAspect;
        [DI] readonly WorkstationsAspect _workstationsAspect;
        [DI] readonly GuestGroupAspect _guestGroupAspect;

        private ProtoItExc _startWaitingTakeOrderIt;
        private ProtoItExc _startWaitingOrderIt;

        public void Init(IProtoSystems systems)
        {
            _startWaitingTakeOrderIt = new(new[]
            {
                typeof(GroupArrivedEvent)
            }, new[] { typeof(TimerComponent), typeof(WaitingTakeOrderTag), typeof(WaitingOrderTag),
                typeof(GuestServicedTag) });

            _startWaitingOrderIt = new(new[]
            {
                typeof(GuestGroupTag), typeof(WaitingTakeOrderTag), typeof(InteractedEvent)
            }, new[] { typeof(WaitingOrderTag) });

            _startWaitingTakeOrderIt.Init(_world);
            _startWaitingOrderIt.Init(_world);
        }

        public void Run()
        {
            foreach (var groupEntity in _startWaitingTakeOrderIt)
            {
                Debug.Log("Старт ожидания");
                _guestGroupAspect.WaitingTakeOrderTagPool.Add(groupEntity);
                _guestGroupAspect.GroupIsWalkingPool.Del(groupEntity);
                ref var timer = ref _baseAspect.TimerPool.Add(groupEntity);
                timer.Duration = 10f;
            }

            foreach (var groupEntity in _startWaitingOrderIt)
            {
                _guestGroupAspect.WaitingTakeOrderTagPool.Del(groupEntity);
                _guestGroupAspect.WaitingOrderTagPool.Add(groupEntity);
                ref var timer = ref _baseAspect.TimerPool.GetOrAdd(groupEntity);
                timer.Elapsed = 0;
                timer.Duration = 45f;
            }
        }

        public void Destroy()
        {
            //throw new System.NotImplementedException();
        }
    }
}