using System;
using Game.Scripts.Aspects;
using Leopotam.EcsProto;
using Leopotam.EcsProto.QoL;
using UnityEngine;

public class PhysicsAspect : ProtoAspectInject
{
    public ProtoPool<PositionComponent> PositionPool;
    public ProtoPool<RigidbodyComponent> RigidbodyPool;
    public ProtoPool<MovementSpeedComponent> MovementSpeedPool;
    public ProtoPool<GridPositionComponent> GridPositionPool;
    public ProtoPool<UnityTransformRef> UnityTransformPool;
}

[Serializable]
public struct PositionComponent : IComponent
{
    public Vector3 Position;
}

[Serializable]
public struct GridPositionComponent : IComponent
{
    public Vector3Int Position;
}

[Serializable]
public struct RigidbodyComponent : IComponent
{
    public Rigidbody Rigidbody;
}

[Serializable]
public struct MovementSpeedComponent : IComponent
{
    public float Value;
}

[Serializable]
public struct UnityTransformRef : IComponent, IUnityAuthoring
{
    public Transform Transform;
    public void Authoring(in ProtoPackedEntityWithWorld entity, GameObject go)
    {
        Transform =  go.transform;
    }
}