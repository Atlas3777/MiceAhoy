using System.Linq;
using Game.Script.Aspects;
using Leopotam.EcsProto;
using Leopotam.EcsProto.QoL;
using UnityEngine;

namespace Game.Scripts.Systems
{
    public class GuestBookTableSystem : IProtoInitSystem, IProtoRunSystem
    {
        [DI] private ProtoWorld _world;
        [DI] private GuestAspect _guestAspect;
        [DI] private WorkstationsAspect _workstationsAspect;
        [DI] private PhysicsAspect _physicsAspect;

        private ProtoIt _guestIterator;
        private ProtoIt _freeTablesIterator;
        private ProtoIt _queueIterator;

        public void Init(IProtoSystems systems)
        {
            _guestIterator = new(new[] { typeof(GuestTag), typeof(NeedsTableTag) });
            _freeTablesIterator = new(new[] { typeof(GuestTableComponent), typeof(GuestTableIsFreeTag) });
            _queueIterator = new(new[] { typeof(QueueComponent) });

            _guestIterator.Init(_world);
            _freeTablesIterator.Init(_world);
            _queueIterator.Init(_world);
        }

        public void Run()
        {
            foreach (var guestEntity in _guestIterator)
            {
                if (!NeedHandleGuest(guestEntity))
                    continue;

                if (!TryGiveGuestTable(guestEntity))
                {
                    if (!_guestAspect.GuestInQueueTagPool.Has(guestEntity))
                    {
                        _guestAspect.GuestEnteringQueueTagPool.Add(guestEntity);
                        Debug.Log($"Гость {guestEntity} отправлен в очередь");
                    }
                }
                else
                {
                    foreach (var queueEntity in _queueIterator)
                    {
                        var queue = _guestAspect.QueueComponentPool.Get(queueEntity).Queue;
                        if (queue.Count > 0 && queue.First() == _world.PackEntityWithWorld(guestEntity))
                        {
                            _guestAspect.UpdateQueueEventPool.Add(queueEntity);
                            Debug.Log("Первый из очереди идёт к столу");
                        }
                    }
                }
            }
        }
        
        private bool NeedHandleGuest(ProtoEntity guestEntity)
        {
            foreach (var queueEntity in _queueIterator)
            {
                var queue = _guestAspect.QueueComponentPool.Get(queueEntity).Queue;
                if (queue.Count == 0) continue;

                var firstInQueue = queue.First();
                if (firstInQueue != _world.PackEntityWithWorld(guestEntity))
                {
                    if (!_guestAspect.GuestInQueueTagPool.Has(guestEntity))
                    {
                        _guestAspect.GuestEnteringQueueTagPool.Add(guestEntity);
                        Debug.Log($"Гость {guestEntity} отправлен в очередь");
                    }

                    return false;
                }
            }
            return true;
        }
        
        private bool TryGiveGuestTable(ProtoEntity guestEntity)
        {
            foreach (var tableEntity in _freeTablesIterator)
            {
                ref var guest = ref _guestAspect.TargetPositionComponentPool.Get(guestEntity);
                ref var table = ref _workstationsAspect.GuestTablePool.Get(tableEntity);

                guest.Table = _world.PackEntityWithWorld(tableEntity);
                table.Guest = _world.PackEntityWithWorld(guestEntity);

                _guestAspect.GuestTableIsFreeTagPool.Del(tableEntity);
                _guestAspect.NeedsTableTagPool.Del(guestEntity);
                _guestAspect.GotTableEventPool.Add(guestEntity);
                _guestAspect.GuestIsWalkingTagPool.Add(guestEntity);

                ref var guestPos = ref _physicsAspect.PositionPool.Get(guestEntity);
                guestPos.Position = _physicsAspect.PositionPool.Get(tableEntity).Position;

                Debug.Log($"Guest {guestEntity} получил стол {tableEntity}");
                return true;
            }
            return false;
        }
    }
}
