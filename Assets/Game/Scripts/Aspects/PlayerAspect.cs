using System;
using Game.Script;
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
    public Type Item;
    public PickableItemWrapper PickableItemVisual;

    public void Clear()
    {
        Item = null;
        PickableItemVisual.PickableItemSprite = null;
        PickableItemVisual.PlateItemSprite = null;
        PickableItemVisual.PickableItemSpriteRenderer.sprite = null;
        if (PickableItemVisual.PlateItemSpriteRenderer)
        {
            PickableItemVisual.PlateItemSpriteRenderer.enabled = false;
        }
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
    private Vector2 LookDirectionOld;

    public Vector2 LookDirection
    {
        get
        {
            if (MoveDirection != Vector2.zero)
            {
                LookDirectionOld = MoveDirection.normalized;
                return MoveDirection.normalized;
            }

            return LookDirectionOld;
        }
    }


    public bool InteractPressed;
    public bool PickPlacePressed;

    public bool RandomSpawnFurniturePressed;

    public bool MoveFurniturePressed;
    public bool IsMoveFurnitureNow;
    public bool IsInPlacementMode;
}