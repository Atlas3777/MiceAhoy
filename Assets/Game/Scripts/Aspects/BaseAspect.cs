using System;
using Leopotam.EcsProto;
using Leopotam.EcsProto.QoL;
using Leopotam.EcsProto.Unity;
using TMPro;
using UnityEngine;

public class BaseAspect : ProtoAspectInject
{
    public ProtoPool<TimerComponent> TimerPool;
    public ProtoPool<TimerCompletedEvent> TimerCompletedPool;
    public ProtoPool<VisualizationInfoComponent> VisualizationInfoComponentPool;
}

public interface IComponent
{
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