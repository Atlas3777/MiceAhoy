using System;
using Game.Script.Aspects;
using Game.Script.Infrastructure;
using Game.Scripts.Aspects;
using Leopotam.EcsProto;
using Leopotam.EcsProto.QoL;
using UnityEngine;

public class EndGameSystem : IProtoInitSystem, IProtoRunSystem
{
    [DI] GuestAspect _guestAspect;
    [DI] ProtoWorld _world;
    private ProtoIt _it;
    private ProtoIt _itWin;

    public event Action<GameState> EndGame;
    
    public void Init(IProtoSystems systems)
    {
        _it = new(new[] { typeof(WaitingOrderTag), typeof(TimerCompletedEvent)});
        _itWin = new(new[] { typeof(GuestServedEvent) });
        _it.Init(_world);
        _itWin.Init(_world);
    }

    public void Run()
    {
        foreach (var guestEntity in _itWin)
        {
            EndGame?.Invoke(GameState.Win);
        }
        
        foreach (var guestEntity in _it)
        {
            Debug.LogError("ПРОЕБАЛИ ожидание заказа");
            _guestAspect.WaitingOrderTagPool.Del(guestEntity);
            _guestAspect.GuestServedEventPool.GetOrAdd(guestEntity);
            _guestAspect.GuestServicedTagPool.GetOrAdd(guestEntity);
            _guestAspect.GuestIsWalkingTagPool.Add(guestEntity);
            
            EndGame?.Invoke(GameState.Lose);
        }
    }
}