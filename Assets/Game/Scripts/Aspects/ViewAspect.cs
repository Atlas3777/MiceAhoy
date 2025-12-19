using System;
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
    public Image Image;
    public bool IsActive;
    public Gradient Gradient;

    public void HideComponent()
    {
        Image.enabled = false;
        IsActive = false;
    }

    public void ShowComponent()
    {
        Image.enabled = true;
        IsActive = true;
    }
}