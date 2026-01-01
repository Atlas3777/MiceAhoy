using Game.Scripts.Aspects;
using Leopotam.EcsProto;
using Leopotam.EcsProto.QoL;
using UnityEngine;

internal class PlayerTargetSystem : IProtoInitSystem, IProtoRunSystem
{
    [DI] private ProtoWorld _world;
    [DI] private PlayerAspect _playerAspect;
    [DI] private BaseAspect _baseAspect;
    [DI] private PhysicsAspect _physicsAspect;
    [DI] private WorkstationsAspect _workstationsAspect;

    private ProtoIt _iteratorInteractable;
    private ProtoIt _iteratorPlayer;

    private const float InteractionRange = 2.0f;
    private const float InteractionAngle = 60f;

    public void Init(IProtoSystems systems) {
        _iteratorInteractable = new ProtoIt(new[] { typeof(InteractableComponent), typeof(PositionComponent) });
        _iteratorPlayer = new ProtoIt(new[] { typeof(PlayerInputComponent), typeof(PositionComponent) });

        _iteratorInteractable.Init(_world);
        _iteratorPlayer.Init(_world);
    }

    public void Run() {
        foreach (var entityPlayer in _iteratorPlayer) {
            ref var playerPos = ref _physicsAspect.PositionPool.Get(entityPlayer).Position;
            ref var playerInput = ref _playerAspect.InputRawPool.Get(entityPlayer);

            ProtoEntity bestTarget = default;
            bool targetFound = false;
            float minAngle = float.MaxValue;

            foreach (var entityInteractable in _iteratorInteractable) {
                ref var targetPos = ref _physicsAspect.PositionPool.Get(entityInteractable).Position;
                
                // 1. Быстрая проверка дистанции (sqrMagnitude быстрее чем Distance)
                Vector3 directionToTarget = targetPos - playerPos;
                float sqrDist = directionToTarget.sqrMagnitude;

                if (sqrDist > InteractionRange * InteractionRange) continue;

                directionToTarget.y = 0;
                var rr = playerInput.LookDirection;
                var rrr = new Vector3(rr.x, 0, rr.y);
                var angle = Vector3.Angle(rrr, directionToTarget);

                if (angle < InteractionAngle) {
                    if (angle < minAngle) {
                        minAngle = angle;
                        bestTarget = entityInteractable;
                        targetFound = true;
                    }
                }
            }
            if(targetFound)
                _baseAspect.SelectedByPlayerTagPool.GetOrAdd(bestTarget);

            if (targetFound && playerInput.InteractPressed) {
                HandleInteraction(entityPlayer, bestTarget);
            }
        }
    }

    private void HandleInteraction(ProtoEntity player, ProtoEntity target) {
        if (!_workstationsAspect.PickPlaceEventPool.Has(target)) {
            ref var evt = ref _workstationsAspect.PickPlaceEventPool.Add(target);
            evt.Invoker = _world.PackEntityWithWorld(player);
        }
    }
}