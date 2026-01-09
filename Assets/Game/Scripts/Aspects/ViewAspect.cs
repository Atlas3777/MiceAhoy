using System;
using Game.Scripts.Aspects;
using Leopotam.EcsProto;
using Leopotam.EcsProto.QoL;
using Leopotam.EcsProto.Unity;
using UnityEngine;
using UnityEngine.UI;

public class ViewAspect : ProtoAspectInject
{
    public ProtoPool<ProgressBarComponent> ProgressBarPool;
}

[Serializable]
public struct ProgressBarComponent : IComponent
{
    public bool isDecreasing;
    public bool useGradient;
    public Image Image;
    public CanvasGroup canvasGroup;
    public bool IsActive;
    public Gradient Gradient;

    public void HideComponent()
    {
        if(canvasGroup)
            canvasGroup.alpha = 0;
        Image.enabled = false;
        IsActive = false;
    }

    public void ShowComponent()
    {
        if (canvasGroup)
            canvasGroup.alpha = 1;
        Image.enabled = true;
        IsActive = true;
    }
}