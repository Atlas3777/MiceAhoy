using Leopotam.EcsProto.QoL;
using Leopotam.EcsProto;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class SyncGridPositionSystem : IProtoInitSystem, IProtoRunSystem, IProtoDestroySystem
{
    [DI] readonly PlacementAspect _placementAspect;
    [DI] readonly PhysicsAspect _physicsAspect;
    [DI] readonly ProtoWorld _world;

    private PlacementGrid worldGrid;
    private ProtoIt _iteratorEvent;

    public SyncGridPositionSystem(PlacementGrid placementGrid) =>
        worldGrid = placementGrid;

    public void Init(IProtoSystems systems)
    {
        _iteratorEvent = new(new[] { typeof(SyncGridPositionEvent), typeof(GridPositionComponent), typeof(PositionComponent)});
        _iteratorEvent.Init(_world);
    }

    public void Run()
    {
        foreach (var eventEntity in _iteratorEvent)
        {
            ref var syncComponent = ref _placementAspect.SyncGridPositionEventPool.Get(eventEntity);
            var posInGrid = new Vector3Int(Mathf.RoundToInt(syncComponent.transform.position.x / worldGrid.PlacementZoneCellSize.x),0,
                Mathf.RoundToInt(syncComponent.transform.position.z / worldGrid.PlacementZoneCellSize.z))
                - worldGrid.PlacementZoneIndexStart;
            worldGrid.AddElement(posInGrid);
            ref var gridPositionComponent = ref _physicsAspect.GridPositionPool.Get(eventEntity);
            gridPositionComponent.Position = posInGrid;
            ref var pos = ref _physicsAspect.PositionPool.Get(eventEntity);
            pos.Position = syncComponent.transform.position;

            _placementAspect.SyncGridPositionEventPool.DelIfExists(eventEntity);
        }
    }

    public void Destroy()
    {
        _iteratorEvent = null;
    }
}
