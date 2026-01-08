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
                if (queue.Count == 0)
                    continue;
                
                var packed = queue.Peek();
                if (!packed.TryUnpack(out _, out var guest))
                    continue;
                
                Debug.Log("Мы тут");
                _guestAspect.WaitingOrderTagPool.Add(guest);
                _guestAspect.GuestServicedTagPool.Add(guest);
                _guestAspect.GuestServedEventPool.Add(guest);
                _guestAspect.NeedsTableTagPool.Del(guest);
                _guestAspect.QueueNeedsUpdateTagPool.Add(queueEntity);
                _guestAspect.UpdateQueueEventPool.Add(queueEntity);
            }
            
            foreach (var queueEntity in _queueWaitingIt)
            {
                ref var queue = ref _guestAspect.QueueComponentPool.Get(queueEntity).Queue;
                if (queue.Count == 0)
                    continue;

                if (!_baseAspect.TimerPool.Has(queueEntity))
                {
                    ref var timer = ref _baseAspect.TimerPool.Add(queueEntity);
                    timer.Elapsed = 0f;
                    timer.Completed = false;
                    timer.Duration = _guestAspect.QueueWaitingTimeComponentPool
                        .Get(queueEntity).WaitingTime;
                }
            }
        }
    }
}