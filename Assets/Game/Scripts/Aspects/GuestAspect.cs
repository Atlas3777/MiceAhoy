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
        public ProtoPool<GuestGroupComponent> GuestGroupComponentPool;
        public ProtoPool<GuestGameObjectRefComponent> GuestGameObjectRefComponentPool;
        public ProtoPool<NavMeshAgentComponent> NavMeshAgentComponentPool;
        
        public ProtoPool<ReachedTargetPositionEvent> ReachedTargetPositionEventPool;
        
        public ProtoPool<GuestTag> GuestTagPool;
        public ProtoPool<GuestServicedTag> GuestServicedPool;
        public ProtoPool<GuestTableIsFreeTag> GuestTableIsFreeTagPool;
        public ProtoPool<GuestIsWalkingTag> GuestIsWalkingTagPool;
        public ProtoPool<GuestDidArriveTag>  GuestDidArriveTagPool;
    }

    [Serializable]
    public struct GuestTag : IComponent
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
    public struct GuestGroupComponent : IComponent
    {
        public ProtoPackedEntityWithWorld GuestGroup;
    }

    [Serializable]
    public struct TargetPositionComponent : IComponent
    {
        public ProtoPackedEntityWithWorld Table;
        public Vector2 Position;
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
}