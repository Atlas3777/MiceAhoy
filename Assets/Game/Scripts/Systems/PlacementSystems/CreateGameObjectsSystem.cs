using Leopotam.EcsProto.QoL;
using Leopotam.EcsProto;
using UnityEngine;
using System;

public class CreateGameObjectsSystem : IProtoInitSystem, IProtoRunSystem, IProtoDestroySystem
{
    [DI] readonly PlacementAspect _placementAspect;
    [DI] readonly PhysicsAspect _physicsAspect;

    private PlacementGrid worldGrid;
    private ProtoIt _CreateGOIterator;
    private ProtoWorld _world;

    public CreateGameObjectsSystem(PlacementGrid placementGrid)
    {
        worldGrid = placementGrid;
    }

    public void Init(IProtoSystems systems)
    {
        _world = systems.World();
        _CreateGOIterator = new(new[] { typeof(CreateGameObjectEvent), typeof(RigidbodyComponent) });
        _CreateGOIterator.Init(_world);
    }

    public void Run()
    {
        foreach (var createEvent in _CreateGOIterator)
        {
            ref var component = ref _placementAspect.CreateGameObjectEventPool.Get(createEvent);
            foreach (var obj in component.objects)
            {
                var furn = GetGameObject(obj.furnitureType);
                var pivotDiff = new Vector2(0, 0);
                worldGrid.TryGetPivotDifference(obj.furnitureType, out pivotDiff);
                var position2D = new Vector2(obj.gridPosition.x * worldGrid.PlacementZoneCellSize.x,
                    obj.gridPosition.y * worldGrid.PlacementZoneCellSize.y) + worldGrid.PlacementZoneCellSize / 2
                    + worldGrid.PlacementZoneWorldStart + pivotDiff;
                GameObject.Instantiate(furn, new Vector3(position2D.x, position2D.y, 0), Quaternion.identity);
                worldGrid.AddElement(obj.gridPosition);
            }
            ref var rig2D = ref _physicsAspect.Rigidbody2DPool.Get(createEvent);
            if (component.destroyInvoker)
            {
                GameObject.Destroy(rig2D.Rigidbody.gameObject);
                _world.DelEntity(createEvent);
            }
            else
            {
                _placementAspect.CreateGameObjectEventPool.DelIfExists(createEvent);
            }
        }
    }

    private GameObject GetGameObject(Type type)
    {
        if (worldGrid.TryGetFurniturePrefab(type, out var furniture)) return furniture;
        throw new NotImplementedException();
    }

    public void Destroy()
    {
        _CreateGOIterator = null;
    }
}
