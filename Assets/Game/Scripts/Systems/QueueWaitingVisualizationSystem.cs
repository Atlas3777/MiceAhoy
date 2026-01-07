using System.Linq;
using Game.Script.Aspects;
using Game.Scripts.Aspects;
using Leopotam.EcsProto;
using Leopotam.EcsProto.QoL;
using UnityEngine;

namespace Game.Scripts.Systems
{
    public class QueueWaitingVisualizationSystem : IProtoInitSystem, IProtoRunSystem
    {
        [DI] private GuestAspect _guestAspect;
        [DI] private BaseAspect _baseAspect;
        [DI] private ProtoWorld _world;

        private ProtoIt _queueWaitingIt;
        private ProtoIt _queueTimeoutIt;
        
        public void Init(IProtoSystems systems)
        {
            _queueWaitingIt = new (new[] {typeof(QueueIsNotEmptyTag), typeof(UpdateQueueEvent)});
            _queueTimeoutIt = new ProtoIt(new[] { typeof(QueueIsNotEmptyTag), typeof(TimerCompletedEvent) });
            _queueWaitingIt.Init(_world);
            _queueTimeoutIt.Init(_world);
        }

        public void Run()
        {
            foreach (var queueEntity in _queueTimeoutIt)
            {
                ref var queue = ref _guestAspect.QueueComponentPool.Get(queueEntity).Queue;
                var leavingGuestPacked = queue.Dequeue();
                if (!leavingGuestPacked.TryUnpack(out _, out var unpackedGuest))
                {
                    Debug.LogWarning("Он умер");
                    continue;
                }
                _guestAspect.WaitingOrderTagPool.Add(unpackedGuest);
                _baseAspect.TimerCompletedPool.Add(unpackedGuest);
                _guestAspect.UpdateQueueEventPool.Add(queueEntity);
                _guestAspect.UpdateQueuePositionsEventPool.Add(queueEntity);
            }
            
            foreach (var queueEntity in _queueWaitingIt)
            {
                ref var queue = ref _guestAspect.QueueComponentPool.Get(queueEntity).Queue;
                if (queue.Count == 0)
                {
                    _baseAspect.TimerCompletedPool.GetOrAdd(queueEntity);
                    continue;
                }
                ref var timer = ref _baseAspect.TimerPool.GetOrAdd(queueEntity);
                timer.Elapsed = 0f;
                timer.Duration = _guestAspect.QueueWaitingTimeComponentPool.Get(queueEntity).WaitingTime;
            }
        }
    }
}