using System.Collections.Generic;
using System.Linq;
using Game.Script.Aspects;
using Leopotam.EcsProto;
using Leopotam.EcsProto.QoL;
using UnityEngine;

namespace Game.Scripts.Systems
{
    public class GuestNavigateToQueueSystem : IProtoRunSystem, IProtoInitSystem
    {
        [DI] private ProtoWorld _world;
        [DI] private GuestAspect _guestAspect;
        [DI] private PhysicsAspect _physicsAspect;

        private ProtoIt _enteringGuestsIt;
        private ProtoIt _queueIt;

        private readonly Transform _queueHead;

        public GuestNavigateToQueueSystem(Transform queueHead)
        {
            this._queueHead = queueHead;
        }

        public void Init(IProtoSystems systems)
        {
            _enteringGuestsIt = new(new[] { typeof(GuestTag), typeof(GuestEnteringQueueEvent), });
            _queueIt = new(new[] { typeof(QueueComponent) });
            _enteringGuestsIt.Init(_world);
            _queueIt.Init(_world);
        }

        public void Run()
        {
            var offset = new Vector3(-1.2f, 0, 0);

            foreach (var queueEntity in _queueIt)
            {
                ref var queue = ref _guestAspect.QueueComponentPool.Get(queueEntity).Queue;
                queue ??= new Queue<ProtoPackedEntityWithWorld>();

                foreach (var guestEntity in _enteringGuestsIt)
                {
                    ref var agent = ref _guestAspect.NavMeshAgentComponentPool.Get(guestEntity).Agent;

                    var indexInQueue = queue.Count;
                    var targetPos = _queueHead.position + offset * indexInQueue;

                    agent.SetDestination(targetPos);

                    queue.Enqueue(_world.PackEntityWithWorld(guestEntity));
                    _guestAspect.GuestInQueueTagPool.Add(guestEntity);

                    if (indexInQueue == 0)
                    {
                        _guestAspect.QueueIsNotEmptyTagPool.GetOrAdd(queueEntity);
                        _guestAspect.UpdateQueueVisualEventPool.GetOrAdd(queueEntity);
                    }

                    Debug.Log($"Гость {guestEntity} встал в очередь на позицию {indexInQueue}");
                    _guestAspect.GuestIsWalkingTagPool.GetOrAdd(guestEntity);
                }
            }
        }
    }
}