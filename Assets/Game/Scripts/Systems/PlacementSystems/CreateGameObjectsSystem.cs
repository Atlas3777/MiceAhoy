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
                var pivotDiff = Vector3.zero;
                worldGrid.TryGetPivotDifference(obj.furnitureType, out pivotDiff);
                var position3D = new Vector3(obj.gridPosition.x * worldGrid.PlacementZoneCellSize.x,0,
                    obj.gridPosition.z * worldGrid.PlacementZoneCellSize.z) //+ worldGrid.PlacementZoneCellSize / 2
                    + worldGrid.PlacementZoneWorldStart + pivotDiff;
                GameObject.Instantiate(furn, new Vector3(position3D.x, position3D.y, position3D.z), Quaternion.identity);
                worldGrid.AddElement(obj.gridPosition);
            }
            ref var rig = ref _physicsAspect.RigidbodyPool.Get(createEvent);
            if (component.destroyInvoker)
            {
                GameObject.Destroy(rig.Rigidbody.gameObject);
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
