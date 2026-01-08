using System;
using Game.Script.Aspects;
using Game.Scripts.Systems;
using Leopotam.EcsProto;
using Leopotam.EcsProto.QoL;
using TMPro;
using UnityEngine;

namespace Game.Scripts.Aspects
{
    public interface IComponent
    {
    }

    public class BaseAspect : ProtoAspectInject
    {
        public ProtoPool<TimerComponent> TimerPool;
        public ProtoPool<TimerCompletedEvent> TimerCompletedPool;
        public ProtoPool<VisualizationInfoComponent> VisualizationInfoComponentPool;
        public ProtoPool<SelectedByPlayerEvent> SelectedByPlayerTagPool;
        public ProtoPool<InteractableComponent> InteractableComponentPool;
        public ProtoPool<HolderComponent> HolderPool;
        public ProtoPool<SpawnGuestRequest> SpawnGuestRequestPool;
        public ProtoPool<ReputationRequest> ReputationRequestPool;
        public ProtoPool<PlaySFXRequest> PlaySFXRequestPool;
        public ProtoPool<StartLoopSound> StartLoopSoundPool;
        public ProtoPool<StopLoopSound> StopLoopSoundPool;
        public ProtoPool<ActiveLoopSound> ActiveLoopSoundPool;
        
        public GuestAspect GuestAspect;
        public WorkstationsAspect WorkstationsAspect;
        public ViewAspect ViewAspect;
        public PlayerAspect PlayerAspect;
        public PlacementAspect PlacementAspect;
        public PhysicsAspect PhysicsAspect;
    }
    public struct StartLoopSound { public SoundType SoundType; }
    public struct StopLoopSound { }
    public struct ActiveLoopSound 
    { 
        public SoundType SoundType; 
        public Guid InternalId; 
    }

    public struct PlaySFXRequest 
    {
        public SoundType SoundType;
    }

    public struct ReputationRequest
    {
        public int Diff;
    }
    
    public struct SpawnGuestRequest
    {
        public GuestProfile Profile;
    }

    [Serializable]
    public struct HolderComponent : IComponent
    {
        public GameObject HolderRootGO;
        public ProtoPackedEntityWithWorld ItemEntity;
        public Type Item;
        public GameObject PickableItemGO;
        private PickableItemInfoWrapper ItemInfo;

        public PickableItemInfoWrapper GetItemInfo
        {
            get
            {
                if (ItemInfo == null)
                    ItemInfo = PickableItemGO.GetComponent<PickableItemInfoWrapper>();
                return ItemInfo;
            }
        }
            

        public void Clear()
        {
            Item = null;
            PickableItemGO = null;
        }
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