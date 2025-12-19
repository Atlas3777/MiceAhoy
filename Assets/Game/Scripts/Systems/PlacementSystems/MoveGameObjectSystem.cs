using Leopotam.EcsProto.QoL;
using Leopotam.EcsProto;
using UnityEngine;
using System.ComponentModel;

public class MoveGameObjectSystem : IProtoInitSystem, IProtoRunSystem, IProtoDestroySystem
{
    [DI] readonly PlacementAspect _placementAspect;
    [DI] readonly PhysicsAspect _physicsAspect;

    private PlacementGrid worldGrid;
    private ProtoIt _iterator;
    private ProtoWorld _world;

    public MoveGameObjectSystem(PlacementGrid placementGrid) =>
        worldGrid = placementGrid;

    public void Init(IProtoSystems systems)
    {
        _world = systems.World();
        _iterator = new(new[] { typeof(MoveThisGameObjectEvent), typeof(PlacementTransformComponent), typeof(GridPositionComponent),
        typeof(FurnitureComponent)});
        _iterator.Init(_world);
    }

    public void Run()
    {
        foreach (var entity in _iterator)
        {
            ref var moveComponent = ref _placementAspect.MoveThisGameObjectEventPool.Get(entity);
            ref var transform = ref _placementAspect.PlacementTransformPool.Get(entity);
            ref var gridPosition = ref _physicsAspect.GridPositionPool.Get(entity);
            ref var furnComponent = ref _placementAspect.FurniturePool.Get(entity);

            gridPosition.Position = moveComponent.newPositionInGrid;
            var scaledPosition = new Vector2(gridPosition.Position.x*worldGrid.PlacementZoneCellSize.x,
                gridPosition.Position.y*worldGrid.PlacementZoneCellSize.y);
            var pivotDiff = new Vector2(0, 0);
            worldGrid.TryGetPivotDifference(furnComponent.type.GetType(), out pivotDiff);
            var worldPosition = worldGrid.PlacementZoneWorldStart + scaledPosition + worldGrid.PlacementZoneCellSize / 2 
                + pivotDiff;
            transform.transform.position = new Vector3(worldPosition.x, worldPosition.y, 0);

            _placementAspect.MoveThisGameObjectEventPool.DelIfExists(entity);
        }
    }

    public void Destroy()
    {
        _iterator = null;
    }
}
