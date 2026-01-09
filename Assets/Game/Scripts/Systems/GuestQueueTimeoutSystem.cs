using Game.Script.Aspects;
using Game.Scripts.Aspects;
using Leopotam.EcsProto;
using Leopotam.EcsProto.QoL;
using UnityEngine;

namespace Game.Scripts.Systems
{
    public class GuestQueueTimeoutSystem : IProtoInitSystem, IProtoRunSystem
    {
        [DI] private GuestAspect _guestAspect;
        [DI] private BaseAspect _baseAspect;
        [DI] private ProtoWorld _world;

        private ProtoIt _timeoutGuests;
        private ProtoIt _queueIt;

        public void Init(IProtoSystems systems)
        {
            _timeoutGuests = new ProtoIt(new[] { typeof(GuestInQueueTag), typeof(TimerCompletedEvent) });
            _queueIt = new(new[] { typeof(QueueComponent), typeof (QueueIsNotEmptyTag) });
            _queueIt.Init(_world);
            _timeoutGuests.Init(_world);
        }

        public void Run()
        {
            foreach (var queueEntity in _queueIt)
            {
                ref var queue = ref _guestAspect.QueueComponentPool.Get(queueEntity).Queue;
                var firstGuestPacked = queue.Peek();
                if (!firstGuestPacked.TryUnpack(out _, out var unpackedGuest))
                {
                    Debug.LogWarning("Первый гость мёртв");
                    continue;
                }
                foreach (var timeoutGuestEntity in _timeoutGuests)
                {
                    if (timeoutGuestEntity == unpackedGuest)
                    {
                        _guestAspect.WaitingOrderTagPool.Add(timeoutGuestEntity);
                        _guestAspect.GuestServicedTagPool.Add(timeoutGuestEntity);
                        _guestAspect.GuestServedEventPool.Add(timeoutGuestEntity);
                        _guestAspect.NeedsTableTagPool.Del(timeoutGuestEntity);
                        Debug.Log("Очередь в таймауте, гость уходит");
                        _guestAspect.QueueNeedsUpdateTagPool.Add(queueEntity);
                        _guestAspect.UpdateQueueVisualEventPool.GetOrAdd(queueEntity);
                        _guestAspect.GuestViewComponentPool.Get(timeoutGuestEntity).view.canvasGroup.alpha = 0;
                        break;
                    }
                }
            }
        }
    }
}