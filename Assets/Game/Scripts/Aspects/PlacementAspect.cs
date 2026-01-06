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
    public ProtoPool<MoveThisFurnitureTag> MoveThisFurnitureEventPool;
    public ProtoPool<CreateGameObjectEvent> CreateGameObjectEventPool;
    public ProtoPool<MoveThisGameObjectEvent> MoveThisGameObjectEventPool;
    public ProtoPool<SyncGridPositionEvent> SyncGridPositionEventPool;
    public ProtoPool<SpawnFurnitureEvent> SpawnFurnitureEventPool;
    public ProtoPool<SpawnerTag> SpawnerTagPool;
    public ProtoPool<CreateSpawnersEvent> CreateSpawnersEventPool;
    public ProtoPool<DestroyAllSpawnersEvent> DestroyAllSpawnersEventPool;
    public ProtoPool<PlacementModeTag> PlacementModeTagPool;
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
    public Transform Transform;
    public void Authoring(in ProtoPackedEntityWithWorld entity, GameObject go)
    {
        Transform = go.transform;
    }
}

[Serializable]
public struct SpawnerTag : IComponent
{
    [SerializeReference, SubclassSelector]
    public WorkstationItem spawnObjectType;
}

public struct SpawnFurnitureEvent
{
}

public struct CreateSpawnersEvent
{
}

public struct DestroyAllSpawnersEvent
{

}

public struct PlacementModeTag
{

}