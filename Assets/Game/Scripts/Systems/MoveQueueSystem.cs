using Game.Script.Aspects;
using Game.Scripts.Aspects;
using Leopotam.EcsProto;
using Leopotam.EcsProto.QoL;
using UnityEngine;

namespace Game.Scripts.Systems
{
    public class MoveQueueSystem : IProtoInitSystem, IProtoRunSystem
    {
        [DI] private GuestAspect _guestAspect;
        [DI] private BaseAspect _baseAspect;
        [DI] private PhysicsAspect _physicsAspect;
        [DI] private ProtoWorld _world;
        
        private ProtoIt _queueIt;

        private readonly Transform _queueHead;

        public MoveQueueSystem(Transform queueHead)
        {
            this._queueHead = queueHead;
        }
        
        public void Init(IProtoSystems systems)
        {
            _queueIt = new(new[] { typeof(UpdateQueueEvent) });
            _queueIt.Init(_world);
        }

        public void Run()
        {
            foreach (var queueEntity in _queueIt)
            {
                ref var queue = ref _guestAspect.QueueComponentPool.Get(queueEntity).Queue;
                var first = queue.Dequeue();
                if (!first.TryUnpack(out _, out var firstGuest))
                {
                    Debug.LogWarning("Гость скончался прям в очереди");
                    continue;
                }

                var prevGuestPlace = _queueHead.position;
                Debug.LogWarning(prevGuestPlace);
                _guestAspect.GuestLeavingQueueEventPool.Add(firstGuest);
                _guestAspect.GuestInQueueTagPool.Del(firstGuest);
                foreach (var packedGuest in queue)
                {
                    if (!packedGuest.TryUnpack(out _, out var guest))
                    {
                        Debug.LogWarning("Гость скончался прям в очереди");
                        continue;
                    }
                    
                    ref var agent = ref _guestAspect.NavMeshAgentComponentPool.Get(guest).Agent;
                    agent.SetDestination(prevGuestPlace);
                    prevGuestPlace = _physicsAspect.PositionPool.Get(guest).Position;
                }
                _guestAspect.UpdateQueueEventPool.Del(queueEntity);
                Debug.Log("Очередь продвинулась");
            }
        }
    }
}