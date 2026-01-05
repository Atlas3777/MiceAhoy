using Game.Script.Aspects;
using Game.Scripts.Aspects;
using Leopotam.EcsProto;
using Leopotam.EcsProto.QoL;
using UnityEngine;

public class AngryGuestLeaveSystem : IProtoInitSystem, IProtoRunSystem
{
    [DI] GuestAspect _guestAspect;
    [DI] ProtoWorld _world;
    private ProtoIt _it;
    
    public void Init(IProtoSystems systems)
    {
        _it = new(new[] { typeof(WaitingOrderTag), typeof(TimerCompletedEvent)});
        _it.Init(_world);
    }

    public void Run()
    {
        foreach (var guestEntity in _it)
        {
            Debug.LogError("ПРОЕБАЛИ ожидание заказа");
            
            _guestAspect.WaitingOrderTagPool.Del(guestEntity);
            _guestAspect.GuestServedEventPool.GetOrAdd(guestEntity);
            _guestAspect.GuestServicedTagPool.GetOrAdd(guestEntity);
            _guestAspect.GuestIsWalkingTagPool.Add(guestEntity);
        }
    }
}