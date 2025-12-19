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
    private List<Type> spawnerTypes;
    private System.Random rnd;

    public RandomSpawnerPositionSystem(PlacementGrid placementGrid)
    {
        worldGrid = placementGrid;
        spawnerTypes = worldGrid.GetWorkStationTypes().Where(t=> typeof(Spawner).IsAssignableFrom(t)).ToList();
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
            createGO.destroyInvoker = false;

            createGO.objects ??= new();
            for (int i = 0; i < 3; i++)
            {
                var cell = new Vector2Int(rnd.Next(worldGrid.PlacementZoneSize.x),
                    rnd.Next(worldGrid.PlacementZoneSize.y));
                var type = spawnerTypes[rnd.Next(spawnerTypes.Count)];
                if (worldGrid.IsValidEmptyCell(cell))
                    createGO.objects.Add((type,cell));
            }
            _placementAspect.CreateSpawnersEventPool.DelIfExists(entityEvent);
        }
    }

    public void Destroy()
    {
        _iteratorEvent = null;
    }
}
