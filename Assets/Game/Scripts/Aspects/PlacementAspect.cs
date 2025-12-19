using System;
using System.Collections.Generic;
using Leopotam.EcsProto;
using Leopotam.EcsProto.QoL;
using Leopotam.EcsProto.Unity;
using UnityEngine;

internal class PlacementAspect : ProtoAspectInject
{
    public ProtoPool<FurnitureComponent> FurniturePool;
    public ProtoPool<PlacementTransformComponent> PlacementTransformPool;
    public ProtoPool<MoveThisFurnitureTag> MoveThisFurnitureEventPool;
    public ProtoPool<CreateGameObjectEvent> CreateGameObjectEventPool;
    public ProtoPool<MoveThisGameObjectEvent> MoveThisGameObjectEventPool;
    public ProtoPool<SyncGridPositionEvent> SyncMyGridPositionEventPool;
    public ProtoPool<SpawnFurnitureEvent> SpawnFurnitureEventPool;
    public ProtoPool<SpawnerTag> SpawnerTagPool;
    public ProtoPool<CreateSpawnersEvent> CreateSpawnersEventPool;
    public ProtoPool<DestroyAllSpawnersEvent> DestroyAllSpawnersEventPool;
}

[Serializable]
public struct PlacementTransformComponent : IComponent
{
    public Transform transform;
}
[Serializable]
public struct FurnitureComponent : IComponent
{
    [SerializeReference,SubclassSelector]
    public WorkstationItem type;
}

public struct MoveThisFurnitureTag
{
    public ProtoPackedEntityWithWorld Invoker;
}

internal struct CreateGameObjectEvent
{
    public List<(Type furnitureType, Vector2Int gridPosition)> objects;
    public bool destroyInvoker;
}

internal struct MoveThisGameObjectEvent
{
    public Vector2Int newPositionInGrid;
}

[Serializable]
public struct SyncGridPositionEvent : IComponent
{
    public Vector2Int entityGridPositions;
}

[Serializable]
public struct SpawnerTag : IComponent
{
    [SerializeReference, SubclassSelector]
    public WorkstationItem spawnObjectType;
}

internal struct SpawnFurnitureEvent
{
}

internal struct CreateSpawnersEvent
{
}

internal struct DestroyAllSpawnersEvent
{

}