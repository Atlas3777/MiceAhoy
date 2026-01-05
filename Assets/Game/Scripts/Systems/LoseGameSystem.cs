using System;
using Game.Script.Aspects;
using Game.Scripts.Aspects;
using Leopotam.EcsProto;
using Leopotam.EcsProto.QoL;
using UnityEngine;

public class LoseGameSystem : IProtoInitSystem, IProtoRunSystem
{
    [DI] GuestAspect _guestAspect;
    [DI] GuestGroupAspect _guestGroupAspect;
    [DI] ProtoWorld _world;
    private ProtoIt _it;
    private ProtoIt _it2;
    private ProtoIt _itWin;

    public event Action EndGame;
    
    public void Init(IProtoSystems systems)
    {
        _it = new(new[] { typeof(WaitingOrderTag), typeof(TimerCompletedEvent)});
        _it2 = new(new[] { typeof(WaitingTakeOrderTag), typeof(TimerCompletedEvent)});
        _itWin = new(new[] { typeof(GuestGroupServedEvent) });
        _it.Init(_world);
        _it2.Init(_world);
        _itWin.Init(_world);
    }

    public void Run()
    {
        
    }
}