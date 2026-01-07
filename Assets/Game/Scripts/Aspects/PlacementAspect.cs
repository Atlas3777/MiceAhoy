using System;
using System.Collections.Generic;
using Game.Scripts.Aspects;
using Leopotam.EcsProto;
using Leopotam.EcsProto.QoL;
using Leopotam.EcsProto.Unity;
using UnityEngine;
using UnityEngine.Serialization;

public class PlacementAspect : ProtoAspectInject
{
    public ProtoPool<FurnitureComponent> FurniturePool;
    public ProtoPool<MoveThisFurnitureTag> MoveThisFurnitureTagPool;
    public ProtoPool<CreateGameObjectEvent> CreateGameObjectEventPool;
    public ProtoPool<MoveThisGameObjectEvent> MoveThisGameObjectEventPool;
    public ProtoPool<SyncGridPositionEvent> SyncGridPositionEventPool;
    public ProtoPool<SpawnFurnitureEvent> SpawnFurnitureEventPool;
    public ProtoPool<SpawnerTag> SpawnerTagPool;
    public ProtoPool<CreateSpawnersEvent> CreateSpawnersEventPool;
    public ProtoPool<ActivateAllSpawnersEvent> ActivateAllSpawnersEventPool;
    public ProtoPool<PlacementModeTag> PlacementModeTagPool;
    public ProtoPool<TransformComponent> PlacementTransformPool;
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

public struct CreateGameObjectEvent
{
    public List<(Type furnitureType, Vector3Int gridPosition)> objects;
    public bool destroyInvoker;
}

public struct MoveThisGameObjectEvent
{
    public Vector3Int newPositionInGrid;
}

[Serializable]
public struct SyncGridPositionEvent : IComponent, IUnityAuthoring
{
    public Transform transform;
    public void Authoring(in ProtoPackedEntityWithWorld entity, GameObject go)
    {
        transform = go.transform;
    }
}

[Serializable]
public struct SpawnerTag : IComponent
{
    [SerializeReference, SubclassSelector]
    public WorkstationItem spawnObjectType;
}

[Serializable]
public struct TransformComponent : IComponent
{
    public Transform transform;
}

public struct SpawnFurnitureEvent
{
}

public struct CreateSpawnersEvent
{
    public Type spawnerType;
}

public struct ActivateAllSpawnersEvent
{

}

public struct PlacementModeTag
{

}