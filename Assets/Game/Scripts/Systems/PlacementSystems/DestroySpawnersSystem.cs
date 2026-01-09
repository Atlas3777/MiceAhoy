using Leopotam.EcsProto.QoL;
using Leopotam.EcsProto;
using UnityEngine;

public class DestroySpawnersSystem : IProtoInitSystem, IProtoRunSystem, IProtoDestroySystem
{
    [DI] readonly PhysicsAspect _physicsAspect;
    [DI] readonly PlacementAspect _placementAspect;

    private PlacementGrid worldGrid;
    private ProtoIt _iteratorEvent;
    private ProtoIt _iteratorSpawners;
    private ProtoWorld _world;

    public DestroySpawnersSystem(PlacementGrid placementGrid) =>
        worldGrid = placementGrid;

    public void Init(IProtoSystems systems)
    {
        _world = systems.World();
        _iteratorEvent = new(new[] { typeof(ActivateAllSpawnersEvent)});
        _iteratorSpawners = new(new[] { typeof(SpawnerTag) });
        _iteratorEvent.Init(_world);
        _iteratorSpawners.Init(_world);
    }

    public void Run()
    {
        foreach(var entityEvent in _iteratorEvent)
        {
            foreach(var spawner in _iteratorSpawners)
            {
                ref var gridPos = ref _physicsAspect.GridPositionPool.Get(spawner);
                ref var rig2D = ref _physicsAspect.RigidbodyPool.Get(spawner);
                if (!_placementAspect.SpawnFurnitureEventPool.Has(spawner))
                    _placementAspect.SpawnFurnitureEventPool.Add(spawner);
            }
            _placementAspect.ActivateAllSpawnersEventPool.DelIfExists(entityEvent);
        }
    }

    public void Destroy()
    {
        _iteratorEvent = null;
        _iteratorSpawners = null;
    }
}
