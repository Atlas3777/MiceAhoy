using Leopotam.EcsProto.QoL;
using Leopotam.EcsProto;
using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;

public class RandomSpawnerPositionSystem : IProtoInitSystem, IProtoRunSystem, IProtoDestroySystem
{
    [DI] readonly PlacementAspect _placementAspect;

    private PlacementGrid worldGrid;
    private ProtoIt _iteratorEvent;
    private ProtoWorld _world;
    private System.Random rnd;

    public RandomSpawnerPositionSystem(PlacementGrid placementGrid)
    {
        worldGrid = placementGrid;
        rnd = new();
    }

    public void Init(IProtoSystems systems)
    {
        _world = systems.World();
        _iteratorEvent = new(new[] { typeof(CreateSpawnersEvent) });
        _iteratorEvent.Init(_world);
    }

    public void Run()
    { 
        foreach (var entityEvent in _iteratorEvent) 
        {
            if (!_placementAspect.CreateGameObjectEventPool.Has(entityEvent))
                _placementAspect.CreateGameObjectEventPool.Add(entityEvent);
            ref var createGO = ref _placementAspect.CreateGameObjectEventPool.Get(entityEvent);
            ref var spawnerEvent = ref _placementAspect.CreateSpawnersEventPool.Get(entityEvent);
            createGO.destroyInvoker = false;

            createGO.objects ??= new();
            for (int i = 0; i < 3; i++)
            {
                var cell = new Vector3Int(rnd.Next(worldGrid.PlacementZoneSize.x), 0,
                    rnd.Next(worldGrid.PlacementZoneSize.z));
                var type = spawnerEvent.spawnerType;
                if (worldGrid.IsValidEmptyCell(cell))
                {
                    createGO.objects.Add((type, cell));
                    break;
                }
            }
            _placementAspect.CreateSpawnersEventPool.DelIfExists(entityEvent);
        }
    }

    public void Destroy()
    {
        _iteratorEvent = null;
    }
}
