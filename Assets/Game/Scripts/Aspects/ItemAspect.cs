using System;
using Game.Script;
using Leopotam.EcsProto;
using Leopotam.EcsProto.QoL;

public class ItemAspect : ProtoAspectInject
{
    public ProtoPool<MeatComponent> MeatPool;
    public ProtoPool<ItemComponent> ItemPool;
    public ProtoPool<RawMeatComponent> RawMeatPool;
}


public struct ItemComponent
{
    public PickableItem PickupItemType;
}

[Serializable]
public struct RawMeatComponent
{
}

[Serializable]
public struct MeatComponent
{
}