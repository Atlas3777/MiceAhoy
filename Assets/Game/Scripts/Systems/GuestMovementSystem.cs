using Game.Script.Aspects;
using Leopotam.EcsProto;
using Leopotam.EcsProto.QoL;
using UnityEngine;

namespace Game.Scripts.Systems
{
    public class GuestMovementSystem : IProtoInitSystem, IProtoRunSystem, IProtoDestroySystem
    {
        [DI] private readonly GuestAspect _guestAspect;
        [DI] private readonly PhysicsAspect _physicsAspect;

        [DI] private ProtoWorld _world;
        private ProtoIt _moveIterator;

        public void Init(IProtoSystems systems)
        {
            _moveIterator = new(new[]
            {
                typeof(GuestTag), typeof(PositionComponent), typeof(GuestIsWalkingTag),
                typeof(TargetPositionComponent), typeof(MovementSpeedComponent)
            });
            _moveIterator.Init(_world);
        }

        public void Run()
        {
            foreach (var guestEntity in _moveIterator)
            {
                ref var agent = ref _guestAspect.NavMeshAgentComponentPool.Get(guestEntity).Agent;

                if (!agent.pathPending && agent.remainingDistance < 0.3f)
                {
                    _guestAspect.ReachedTargetPositionEventPool.Add(guestEntity);
                    _guestAspect.GuestIsWalkingTagPool.Del(guestEntity);
                    agent.isStopped = true;
                }
                else
                {
                    agent.isStopped = false;
                }
            }
        }

        public void Destroy()
        {
            _moveIterator = null;
        }
    }
}