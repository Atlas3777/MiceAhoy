using System;
using Game.Scripts.Aspects;
using Leopotam.EcsProto;
using Leopotam.EcsProto.QoL;
using UnityEngine;
using UnityEngine.AI;

namespace Game.Script.Aspects
{
    public class GuestAspect : ProtoAspectInject
    {
        public ProtoPool<TargetPositionComponent> TargetPositionComponentPool;
        public ProtoPool<GuestTableComponent> GuestTablePool;
        public ProtoPool<WantedItemComponent> WantedItemPool;
        public ProtoPool<WantedItemVisualizationComponent> WantedItemVisualizationPool;
        public ProtoPool<GuestGameObjectRefComponent> GuestGameObjectRefComponentPool;
        public ProtoPool<NavMeshAgentComponent> NavMeshAgentComponentPool;
        public ProtoPool<GuestStateComponent> GuestStateComponentPool;
        public ProtoPool<GuestSpawnerComponent> GuestSpawnerComponentPool;
        
        public ProtoPool<ReachedTargetPositionEvent> ReachedTargetPositionEventPool;
        public ProtoPool<GotTableEvent> GotTableEventPool;
        public ProtoPool<GuestServedEvent> GuestServedEventPool;
        
        public ProtoPool<GuestTag> GuestTagPool;
        public ProtoPool<GuestServicedTag> GuestServicedTagPool;
        public ProtoPool<GuestTableIsFreeTag> GuestTableIsFreeTagPool;
        public ProtoPool<GuestIsWalkingTag> GuestIsWalkingTagPool;
        public ProtoPool<NeedsTableTag> NeedsTableTagPool;
        public ProtoPool<GuestDidArriveTag>  GuestDidArriveTagPool;
        public ProtoPool<WaitingOrderTag> WaitingOrderTagPool;
    }

    [Serializable]
    public struct GuestStateComponent : IComponent
    {
        public float Hunger;
    }
    
    [Serializable]
    public struct GuestTag : IComponent
    {
    }

    [Serializable]
    public struct NeedsTableTag : IComponent
    {
    }

    [Serializable]
    public struct WaitingOrderTag : IComponent
    {
    }
    
    [Serializable]
    public struct GuestTableIsFreeTag : IComponent
    {
    }

    [Serializable]
    public struct GuestIsWalkingTag : IComponent
    {
    }

    public struct GuestServicedTag :IComponent
    {
    }

    public struct GuestDidArriveTag :IComponent
    {
    }

    public struct ReachedTargetPositionEvent
    {
    }

    [Serializable]
    public struct GotTableEvent : IComponent
    {
    }

    [Serializable]
    public struct GuestServedEvent : IComponent
    {
    }

    // [Serializable]
    // public struct GuestGroupComponent : IComponent
    // {
    //     public ProtoPackedEntityWithWorld GuestGroup;
    // }

    [Serializable]
    public struct TargetPositionComponent : IComponent
    {
        public ProtoPackedEntityWithWorld Table;
        public Vector3 Position;
    }

    [Serializable]
    public struct WantedItemComponent : IComponent
    {
        [SubclassSelector, SerializeReference] public PickableItem WantedItem;
    }

    [Serializable]
    public struct WantedItemVisualizationComponent : IComponent
    {
        public GameObject Visualization;
    }

    [Serializable]
    public struct GuestGameObjectRefComponent : IComponent
    {
        public GameObject GameObject;
    }

    [Serializable]
    public struct NavMeshAgentComponent : IComponent
    {
        public NavMeshAgent Agent;
    }

    [Serializable]
    public struct GuestSpawnerComponent : IComponent
    {
    }
}