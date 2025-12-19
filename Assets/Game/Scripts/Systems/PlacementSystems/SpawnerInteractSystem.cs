using Leopotam.EcsProto.QoL;
using Leopotam.EcsProto;
using UnityEngine;

public class SpawnerInteractSystem : IProtoInitSystem, IProtoRunSystem, IProtoDestroySystem
{
    [DI] readonly PlacementAspect _placementAspect;
    [DI] readonly PhysicsAspect _physicsAspect;

    private ProtoIt _iteratorSpawner;
    private ProtoWorld _world;

    public void Init(IProtoSystems systems)
    {
        _world = systems.World();
        _iteratorSpawner = new(new[] { typeof(SpawnFurnitureEvent), typeof(SpawnerTag), typeof(GridPositionComponent) });
        _iteratorSpawner.Init(_world);
    }

    public void Run()
    {
        foreach (var spawner in _iteratorSpawner) 
        {
            ref var spawnerTag = ref _placementAspect.SpawnerTagPool.Get(spawner);
            ref var gridPosition = ref _physicsAspect.GridPositionPool.Get(spawner);

            if (!_placementAspect.CreateGameObjectEventPool.Has(spawner))
                _placementAspect.CreateGameObjectEventPool.Add(spawner);
            ref var createComponent = ref _placementAspect.CreateGameObjectEventPool.Get(spawner);
            createComponent.objects = new()
            {
                (spawnerTag.spawnObjectType.GetType(), gridPosition.Position)
            };
            createComponent.destroyInvoker = true;

            _placementAspect.SpawnFurnitureEventPool.DelIfExists(spawner);
        }
    }

    public void Destroy()
    {
        _iteratorSpawner = null;
    }
}
