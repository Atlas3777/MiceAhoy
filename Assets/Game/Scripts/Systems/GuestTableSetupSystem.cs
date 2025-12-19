using System.Collections.Generic;
using Game.Script.Aspects;
using Leopotam.EcsProto;
using Leopotam.EcsProto.QoL;
using UnityEngine;

// Определяем константы
public static class GridConstants
{
    public const float VERTICAL_RAYCAST_DISTANCE = 1.04f;
    public const float HORIZONTAL_RAYCAST_DISTANCE = 1.26f;
}

public class GuestTableSetupSystem : IProtoInitSystem, IProtoRunSystem
{
    [DI] private ProtoWorld _world;
    [DI] private PhysicsAspect _physicsAspect;
    [DI] private GuestAspect _guestAspect;
    
    private ProtoIt _it;

    public void Init(IProtoSystems systems)
    {
        _it = new(new[] { typeof(PlaceWorkstationEvent), typeof(GuestTableComponent), typeof(PositionComponent) });
        _it.Init(_world);
    }

    public void Run()
    {
        foreach (var guestTable in _it)
        {
            ref var pos = ref _physicsAspect.PositionPool.Get(guestTable).Position;
            ref var table = ref _guestAspect.GuestTablePool.Get(guestTable);
            
            var tempPlaces = new List<Vector2>(4);
            var startPoint = pos;

            TryAddPlace(startPoint, Vector2.right, GridConstants.HORIZONTAL_RAYCAST_DISTANCE, tempPlaces);
            TryAddPlace(startPoint, Vector2.left, GridConstants.HORIZONTAL_RAYCAST_DISTANCE, tempPlaces);
            TryAddPlace(startPoint, Vector2.up, GridConstants.VERTICAL_RAYCAST_DISTANCE, tempPlaces);
            TryAddPlace(startPoint, Vector2.down, GridConstants.VERTICAL_RAYCAST_DISTANCE, tempPlaces);
            
            table.guestPlaces = tempPlaces.ToArray();
            
            //table.Guests ??= new List<ProtoPackedEntityWithWorld>();
        }
    }

    private void TryAddPlace(Vector2 origin, Vector2 direction, float distance, List<Vector2> _tempPlaces)
    {
        int layerMask = LayerMask.GetMask("StaticItem");

        RaycastHit2D[] _raycastResults = new RaycastHit2D[2];
        int hitCount = Physics2D.RaycastNonAlloc(origin, direction, _raycastResults, distance, layerMask);
        
        bool isSpaceOccupied = hitCount > 1;

        if (!isSpaceOccupied)
        {
            Vector2 chairPosition = origin + (direction * distance);
            
            _tempPlaces.Add(chairPosition);
        }
    }
}