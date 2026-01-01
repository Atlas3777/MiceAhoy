using System;
using Game.Script.Aspects;
using Leopotam.EcsProto;
using Leopotam.EcsProto.QoL;
using TMPro;
using UnityEngine;

namespace Game.Scripts.Aspects
{
    public interface IComponent { }

    public class BaseAspect : ProtoAspectInject
    {
        public ProtoPool<TimerComponent> TimerPool;
        public ProtoPool<TimerCompletedEvent> TimerCompletedPool;
        public ProtoPool<VisualizationInfoComponent> VisualizationInfoComponentPool;
        public ProtoPool<SelectedByPlayerEvent> SelectedByPlayerTagPool;
        public ProtoPool<InteractableComponent> InteractableComponentPool;
    
        public GuestAspect GuestAspect;
        public GuestGroupAspect  GuestGroupAspect;
        public WorkstationsAspect  WorkstationsAspect;
        public ViewAspect  ViewAspect;
        public PlayerAspect   PlayerAspect;
        public PlacementAspect PlacementAspect;
        public PhysicsAspect PhysicsAspect; 
    }

    public struct SelectedByPlayerEvent
    {
    }

    
    [Serializable]
    public struct InteractableComponent : IComponent
    {
        public GameObject Outline;
        public bool IsActive;
    }
    
    
    [Serializable]
    public struct TimerComponent
    {
        public float Elapsed;
        public float Duration;
        public bool Completed;
    }

    public struct TimerCompletedEvent
    {
    }

    [Serializable]
    public struct VisualizationInfoComponent : IComponent
    {
        [SerializeField] private Canvas AllInfo;
        public VisualizationInfo Info;

        public void Hide() => AllInfo.enabled = false;

        public void Show() => AllInfo.enabled = true;
    }

    [Serializable]
    public struct VisualizationInfo
    {
        public TMP_Text satietyRestoration;
    }
}