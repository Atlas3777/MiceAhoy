using Leopotam.EcsProto;
using Leopotam.EcsProto.QoL;
using UnityEngine;

public class MoveFurnitureSystem : IProtoInitSystem, IProtoRunSystem, IProtoDestroySystem
{ 
    [DI] readonly PlayerAspect _playerAspect;
    [DI] readonly PlacementAspect _placementAspect;
    [DI] readonly PhysicsAspect _physicsAspect;

    private PlacementGrid worldGrid;
    private ProtoIt _iteratorFurniture;
    private ProtoWorld _world;

    public MoveFurnitureSystem(PlacementGrid placementGrid) =>
        worldGrid = placementGrid;

    public void Init(IProtoSystems systems)
    {
        _world = systems.World();
        _iteratorFurniture = new(new[] { typeof(MoveThisFurnitureTag), typeof(GridPositionComponent) });
        _iteratorFurniture.Init(_world);
    }

    public void Run()
    {
        foreach (var furn in _iteratorFurniture)
        {
            ref var moveEvent = ref _placementAspect.MoveThisFurnitureTagPool.Get(furn);
            //�������� ������
            moveEvent.Invoker.TryUnpack(out _, out var playerEntity);
            ref var playerInput = ref _playerAspect.InputRawPool.Get(playerEntity);

            if (playerInput.InteractPressed)
            {
                if (playerInput.IsMoveFurnitureNow)
                {
                    playerInput.IsMoveFurnitureNow = false;
                    _placementAspect.MoveThisFurnitureTagPool.DelIfExists(furn);
                    continue;
                }
                playerInput.IsMoveFurnitureNow = true;
            }

            ref var furnComponent = ref _placementAspect.FurniturePool.Get(furn);
            ref var playerPosition = ref _physicsAspect.PositionPool.Get(playerEntity);
            ref var gridPosition = ref _physicsAspect.GridPositionPool.Get(furn);

            var newFurnPosition = GetNearestGridViewedCell(worldGrid, ref playerInput, playerPosition, gridPosition.Position);
            if (newFurnPosition == gridPosition.Position) continue;
            worldGrid.SwitchElement(gridPosition.Position, newFurnPosition);
            gridPosition.Position = newFurnPosition;

            if (!_placementAspect.MoveThisGameObjectEventPool.Has(furn))
                _placementAspect.MoveThisGameObjectEventPool.Add(furn);
            ref var moveGameObjectEvent = ref _placementAspect.MoveThisGameObjectEventPool.Get(furn);
            moveGameObjectEvent.newPositionInGrid = newFurnPosition;


        }
    }
    

    private Vector3Int GetNearestGridViewedCell(PlacementGrid grid, ref PlayerInputComponent input, 
        PositionComponent pos, Vector3Int defaultVector)
    {
        if (pos.Position.x > grid.PlacementZoneWorldStart.x && pos.Position.x < grid.PlacementZoneWorldStart.x + grid.PlacementZoneSize.x * grid.PlacementZoneCellSize.x)
        {
            if (pos.Position.z > grid.PlacementZoneWorldStart.z && pos.Position.z < grid.PlacementZoneWorldStart.z + grid.PlacementZoneSize.z * grid.PlacementZoneCellSize.z)
            {
                var point = pos.Position - worldGrid.PlacementZoneWorldStart;// - worldGrid.PlacementZoneCellSize / 2;
                var scaledPoint = new Vector2(point.x / worldGrid.PlacementZoneCellSize.x, point.z / worldGrid.PlacementZoneCellSize.z)
                    + input.LookDirection * 5 / 8;
                var foundPos = new Vector2Int(Mathf.RoundToInt(scaledPoint.x), Mathf.RoundToInt(scaledPoint.y));
                var angle = Vector2.SignedAngle(new Vector2(1, 0), input.LookDirection);
                var diff = SwitchLookAngle(angle);
                var res = foundPos + diff;
                var result = new Vector3Int(res.x, 0, res.y);
                if (worldGrid.IsValidEmptyCell(result))
                    return result;
            }
        }
        return defaultVector;
    }
    private Vector2Int SwitchLookAngle(float angle)
    {
        if (angle > -45f / 2 && angle <= 45f / 2)
            return new Vector2Int(1, 0);
        if (angle > 45f / 2 && angle <= 45 + 45f / 2)
            return new Vector2Int(1, 1);
        if (angle > 45 + 45f / 2 && angle <= 90 + 45f / 2)
            return new Vector2Int(0, 1);
        if (angle > 90 + 45f / 2 && angle <= 135 + 45f / 2)
            return new Vector2Int(-1, 1);
        if (angle > -90 + 45f / 2 && angle <= -45f / 2)
            return new Vector2Int(1, -1);
        if (angle > -135 + 45f / 2 && angle <= -90 + 45f / 2)
            return new Vector2Int(0, -1);
        if (angle > -180 + 45f / 2 && angle <= -135 + 45f / 2)
            return new Vector2Int(-1, -1);
        return new Vector2Int(-1, 0);
    }

    public void Destroy()
    {
        _iteratorFurniture = null;
    }
}