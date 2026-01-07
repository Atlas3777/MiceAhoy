using System;
using System.Collections.Generic;
using Game.Scripts;
using Game.Scripts.Aspects;
using Leopotam.EcsProto;
using Leopotam.EcsProto.QoL;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

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
        public ProtoPool<GuestViewComponent> GuestViewComponentPool;
        public ProtoPool<GuestInQueueTag> GuestInQueueTagPool;
        public ProtoPool<GuestEnteringQueueTag> GuestEnteringQueueTagPool;
        
        public ProtoPool<ReachedTargetPositionEvent> ReachedTargetPositionEventPool;
        public ProtoPool<GotTableEvent> GotTableEventPool;
        public ProtoPool<GuestServedEvent> GuestServedEventPool;
        public ProtoPool<UpdateQueueEvent> UpdateQueueEventPool;
        public ProtoPool<GuestLeavingQueueEvent> GuestLeavingQueueEventPool;
        
        public ProtoPool<GuestTag> GuestTagPool;
        public ProtoPool<GuestServicedTag> GuestServicedTagPool;
        public ProtoPool<GuestTableIsFreeTag> GuestTableIsFreeTagPool;
        public ProtoPool<GuestIsWalkingTag> GuestIsWalkingTagPool;
        public ProtoPool<NeedsTableTag> NeedsTableTagPool;
        public ProtoPool<GuestDidArriveTag>  GuestDidArriveTagPool;
        public ProtoPool<WaitingOrderTag> WaitingOrderTagPool;
        public ProtoPool<QueueComponent> QueueComponentPool;
        
    }

    [Serializable]
    public struct GuestViewComponent : IComponent
    {
        public Image HungerBarImage;
        public TMP_Text CurrentHunger;
    }

    [Serializable]
    public struct GuestStateComponent : IComponent
    {
        public float MaxHunger;
        public float Hunger;
        public float WaitingSeconds;
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

    [Serializable]
    public struct GuestServicedTag :IComponent
    {
    }

    [Serializable]
    public struct GuestDidArriveTag :IComponent
    {
    }

    [Serializable]
    public struct GuestInQueueTag : IComponent
    {
    }

    [Serializable]
    public struct GuestEnteringQueueTag : IComponent
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

    [Serializable]
    public struct UpdateQueueEvent : IComponent
    {
    }

    [Serializable]
    public struct GuestLeavingQueueEvent : IComponent
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

    [Serializable]
    public struct QueueComponent : IComponent
    {
        public Queue<ProtoPackedEntityWithWorld> Queue;
    }
}