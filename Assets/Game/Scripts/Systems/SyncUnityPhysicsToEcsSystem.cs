using Leopotam.EcsProto;
using UnityEngine;

public class SyncUnityPhysicsToEcsSystem : IProtoInitSystem, IProtoRunSystem
{
    private PhysicsAspect _physics;
    private ProtoIt _iterator;

    public void Init(IProtoSystems systems)
    {
        var world = systems.World();
        _physics = (PhysicsAspect)world.Aspect(typeof(PhysicsAspect));

        _iterator = new(new[] { typeof(RigidbodyComponent), typeof(PositionComponent) });
        _iterator.Init(world);
    }

    public void Run()
    {
        foreach (var entity in _iterator)
        {
            ref var rb = ref _physics.RigidbodyPool.Get(entity);
            if (!rb.Rigidbody)
                continue;
            ref var pos = ref _physics.PositionPool.Get(entity);
            
            pos.Position = rb.Rigidbody.position;
        }
    }
}