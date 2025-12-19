using Leopotam.EcsProto;
using UnityEngine;

class PlayerMovementSystem : IProtoInitSystem, IProtoRunSystem, IProtoDestroySystem
{
    private PlayerAspect _playerAspect;
    private PhysicsAspect _physicsAspect;
        
    private ProtoIt _iterator;

    public void Init(IProtoSystems systems)
    {
        ProtoWorld world = systems.World();
        _playerAspect = (PlayerAspect)world.Aspect(typeof(PlayerAspect));
        _physicsAspect = (PhysicsAspect)world.Aspect(typeof(PhysicsAspect));
        
        _iterator = new(new[] { typeof(PlayerInputComponent), typeof(PositionComponent) });
        _iterator.Init(world);
    }

    public void Run()
    {
        foreach (ProtoEntity entity in _iterator)
        {
            ref var input = ref _playerAspect.InputRawPool.Get(entity);
            ref var speed = ref _physicsAspect.MovementSpeedPool.Get(entity);
            ref var rigidbody = ref _physicsAspect.Rigidbody2DPool.Get(entity);

            var r = rigidbody.Rigidbody;

            var moveDirection = new  Vector3(input.MoveDirection.x, 0, input.MoveDirection.y);

            
            var desiredVelocity = moveDirection * speed.Value; 

            r.linearVelocity = Vector3.Lerp(r.linearVelocity, desiredVelocity, 0.2f);
        }
    }

    public void Destroy()
    {
        _playerAspect = null;
        _iterator = null;
    }
}