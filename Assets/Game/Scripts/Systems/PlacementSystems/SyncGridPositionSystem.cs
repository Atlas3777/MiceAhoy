using Leopotam.EcsProto.QoL;
using Leopotam.EcsProto;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class SyncGridPositionSystem : IProtoInitSystem, IProtoRunSystem, IProtoDestroySystem
{
    [DI] readonly PlacementAspect _placementAspect;
    [DI] readonly PhysicsAspect _physicsAspect;

    private PlacementGrid worldGrid;
    private ProtoIt _iteratorEvent;
    private ProtoWorld _world;

    public SyncGridPositionSystem(PlacementGrid placementGrid) =>
        worldGrid = placementGrid;

    public void Init(IProtoSystems systems)
    {
        _world = systems.World();
        _iteratorEvent = new(new[] { typeof(SyncGridPositionEvent), typeof(PlacementTransformComponent) });
        _iteratorEvent.Init(_world);
    }

    public void Run()
    {
        foreach (var eventEntity in _iteratorEvent)
        {
            ref var syncComponent = ref _placementAspect.SyncMyGridPositionEventPool.Get(eventEntity);
            ref var transformPosition = ref _placementAspect.PlacementTransformPool.Get(eventEntity);
            var posInGrid = new Vector2Int(Mathf.FloorToInt(transformPosition.transform.position.x / worldGrid.PlacementZoneCellSize.x),
                Mathf.FloorToInt(transformPosition.transform.position.y / worldGrid.PlacementZoneCellSize.y))
                - worldGrid.PlacementZoneIndexStart;
            worldGrid.AddElement(posInGrid);
            ref var gridPositionComponent = ref _physicsAspect.GridPositionPool.Get(eventEntity);
            gridPositionComponent.Position = posInGrid;

            _placementAspect.SyncMyGridPositionEventPool.DelIfExists(eventEntity);
        }
    }

    public void Destroy()
    {
        _iteratorEvent = null;
    }
}
