using System;
using System.Collections.Generic;
using Game.Script;
using Leopotam.EcsProto;
using Leopotam.EcsProto.QoL;
using UnityEngine;

public class WorkstationsAspect : ProtoAspectInject
{
    public ProtoPool<WorkstationsTypeComponent> WorkstationsTypePool;
    public ProtoPool<ItemSourceComponent> ItemSourcePool;
    public ProtoPool<ReceiptProcessorComponent> StovePool;
    public ProtoPool<GuestTableComponent> GuestTablePool;
    
    public ProtoPool<ItemGenerationAvailableTag> ItemGenerationAvailablePool;
    
    public ProtoPool<PickPlaceEvent> PickPlaceEventPool;
    public ProtoPool<ItemPlaceEvent> ItemPlaceEventPool;
    public ProtoPool<ItemPickEvent> ItemPickEventPool;
    public ProtoPool<PlaceWorkstationEvent> PlaceWorkstationEventPool;
    public ProtoPool<InteractedEvent> InteractedEventPool;
}
public struct InteractedEvent 
{
}
[Serializable]
public struct PlaceWorkstationEvent : IComponent
{
}

public struct PickPlaceEvent
{
    public ProtoPackedEntityWithWorld Invoker;
}

public struct ItemPickEvent
{
}

public struct ItemPlaceEvent
{
}

[Serializable]
public struct ItemGenerationAvailableTag : IComponent
{
}

[Serializable]
public struct InteractableComponent : IComponent
{
    public SpriteRenderer SpriteRenderer;
}


[Serializable]
public struct ReceiptProcessorComponent : IComponent
{
}

[Serializable]
public struct ItemSourceComponent : IComponent
{
    [SerializeReference, SubclassSelector] public PickableItem resourceItemType;
}

[Serializable]
public struct GuestTableComponent : IComponent
{
    public Vector2[] guestPlaces;
    public ProtoPackedEntityWithWorld GuestGroup;
}

[Serializable]
public struct WorkstationsTypeComponent : IComponent
{
    [SerializeReference, SubclassSelector] public WorkstationItem workstationType;
}