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
        _iteratorEvent = new(new[] { typeof(SyncGridPositionEvent), typeof(GridPositionComponent), typeof(PositionComponent), typeof(FurnitureComponent)});
        _iteratorEvent.Init(_world);
    }

    public void Run()
    {
        foreach (var eventEntity in _iteratorEvent)
        {
            ref var  syncGridPositionEvent = ref _placementAspect.SyncGridPositionEventPool.Get(eventEntity);
            ref var  positionComponent = ref _physicsAspect.PositionPool.Get(eventEntity);
            ref var  gridPositionComponent = ref _physicsAspect.GridPositionPool.Get(eventEntity);

            var t = syncGridPositionEvent.Transform.position;
            positionComponent.Position = t;
            gridPositionComponent.Position = Vector3Int.RoundToInt(t);
        }
    }

    public void Destroy()
    {
        _iteratorEvent = null;
    }
}
