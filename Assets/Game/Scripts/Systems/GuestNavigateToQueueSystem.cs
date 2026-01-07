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
            _enteringGuestsIt = new(new[] { typeof(GuestTag), typeof(GuestEnteringQueueTag), });
            _queueIt = new(new[] { typeof(QueueComponent) });
            _enteringGuestsIt.Init(_world);
            _queueIt.Init(_world);
        }

        public void Run()
        {
            var offset = new Vector3(0, 0, 1);
            foreach (var queueEntity in _queueIt)
            {
                ref var queue = ref _guestAspect.QueueComponentPool.Get(queueEntity).Queue;
                queue ??= new Queue<ProtoPackedEntityWithWorld>();
                ProtoPackedEntityWithWorld lastGuest = default;
                if (queue.Count > 0)
                    lastGuest = queue.Last();
                foreach (var guestEntity in _enteringGuestsIt)
                {
                    ref var agent = ref _guestAspect.NavMeshAgentComponentPool.Get(guestEntity).Agent;
                    var targetPos = Vector3.zero;
                    if (queue.Count == 0)
                        targetPos = _queueHead.position;
                    if (!lastGuest.TryUnpack(out _, out var unpackedLastGuest))
                    {
                        if (queue.Count > 0)
                        {
                            Debug.LogWarning($"Гость {lastGuest} умер");
                            break;
                        }
                    }
                    if (queue.Count > 0)
                        targetPos = _physicsAspect.PositionPool.Get(unpackedLastGuest).Position + offset;
                    agent.SetDestination(targetPos);
                    ++offset.z;
                    var packed = _world.PackEntityWithWorld(guestEntity);
                    queue.Enqueue(packed);
                    _guestAspect.GuestEnteringQueueTagPool.Del(guestEntity);
                    _guestAspect.GuestInQueueTagPool.Add(guestEntity);
                    Debug.Log("Иду в очередь");
                }
            }
        }
    }
}