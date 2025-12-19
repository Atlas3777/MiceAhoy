using System;
using Game.Script.Aspects;
using Game.Script.Infrastructure;
using Leopotam.EcsProto;
using Leopotam.EcsProto.QoL;
using UnityEngine;

public class EndGameSystem : IProtoInitSystem, IProtoRunSystem
{
    [DI] GuestAspect _guestAspect;
    [DI] GuestGroupAspect _guestGroupAspect;
    [DI] ProtoWorld _world;
    private ProtoIt _it;
    private ProtoIt _it2;
    private ProtoIt _itWin;

    public event Action<GameState> EndGame;
    
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
        foreach (var group in _itWin)
        {
            EndGame?.Invoke(GameState.Win);
        }
        
        foreach (var guestGroupEntity in _it)
        {
            Debug.LogError("ПРОЕБАЛИ ожидание заказа");
            
            EndGame?.Invoke(GameState.Lose);
            
// Получаем компонент группы
            ref var guestGroup = ref _guestGroupAspect.GuestGroupPool.Get(guestGroupEntity);

// Проставляем каждому гостю тег обслуживания
            foreach (var packedGuest in guestGroup.includedGuests)
                if (packedGuest.TryUnpack(out _, out var guestEntity))
                {
                    if (!_guestAspect.GuestServicedPool.Has(guestEntity))
                        _guestAspect.GuestServicedPool.Add(guestEntity);
                }

            _guestGroupAspect.WaitingOrderTagPool.Del(guestGroupEntity);
            _guestGroupAspect.GuestGroupServedEventPool.Add(guestGroupEntity);
            _guestGroupAspect.GuestGroupServedTagPool.Add(guestGroupEntity);
        }

        foreach (var guestGroupEntity in _it2)
        {
            Debug.LogError("ПРОЕБАЛИ не взял заказаз, сука тварь");
            
            EndGame?.Invoke(GameState.Lose);
            
// Получаем компонент группы
            ref var guestGroup = ref _guestGroupAspect.GuestGroupPool.Get(guestGroupEntity);

// Проставляем каждому гостю тег обслуживания
            foreach (var packedGuest in guestGroup.includedGuests)
                if (packedGuest.TryUnpack(out _, out var guestEntity))
                {
                    if (!_guestAspect.GuestServicedPool.Has(guestEntity))
                        _guestAspect.GuestServicedPool.Add(guestEntity);
                }

            _guestGroupAspect.WaitingTakeOrderTagPool.Del(guestGroupEntity);
            _guestGroupAspect.GuestGroupServedEventPool.Add(guestGroupEntity);
            _guestGroupAspect.GuestGroupServedTagPool.Add(guestGroupEntity);
        }
    }
}