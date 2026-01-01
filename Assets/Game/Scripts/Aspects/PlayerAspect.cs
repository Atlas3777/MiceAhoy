using System;
using Game.Script;
using Game.Scripts.Aspects;
using Leopotam.EcsProto;
using Leopotam.EcsProto.QoL;
using Leopotam.EcsProto.Unity;
using UnityEngine;

public class PlayerAspect : ProtoAspectInject
{
    public ProtoPool<PlayerIndexComponent> PlayerIndexPool;
    public ProtoPool<PlayerInputComponent> InputRawPool;
    public ProtoPool<InteractableComponent> InteractablePool;
    public ProtoPool<HolderComponent> HolderPool;
    
    public ProtoPool<HasItemTag> HasItemTagPool;
    
    public ProtoPool<PlayerInitializeEvent> PlayerInitializeEventPool;
}

[Serializable]
public struct PlayerIndexComponent : IComponent
{
    public int PlayerIndex;
}

[Serializable]
public struct PlayerInitializeEvent : IComponent
{
    
}

[Serializable]
public struct HolderComponent : IComponent
{
    public Transform HolderTransform;
    public Type Item;
    public GameObject PickableItemVisual;

    public void Clear()
    {
        Item = null;
        PickableItemVisual = null;
    }
}

[Serializable]
public struct HasItemTag : IComponent
{
}

[Serializable]
public struct PlayerInputComponent : IComponent
{
    public Vector2 MoveDirection; 
    public Vector2 LookDirection;



    public bool InteractPressed;
    public bool PickPlacePressed;

    public bool RandomSpawnFurniturePressed;

    public bool MoveFurniturePressed;
    public bool IsMoveFurnitureNow;
    public bool IsInPlacementMode;
}