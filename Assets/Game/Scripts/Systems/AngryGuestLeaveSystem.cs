using Game.Script.Aspects;
using Game.Scripts.Aspects;
using Leopotam.EcsProto;
using Leopotam.EcsProto.QoL;
using UnityEngine;

public class AngryGuestLeaveSystem : IProtoInitSystem, IProtoRunSystem
{
    [DI] GuestAspect _guestAspect;
    [DI] WorkstationsAspect _workstationsAspect;
    [DI] ProtoWorld _world;
    private ProtoIt _it;
    private ProtoItExc _occupiedTablesIt;
    
    public void Init(IProtoSystems systems)
    {
        _it = new(new[] { typeof(WaitingOrderTag), typeof(TimerCompletedEvent)});
        _occupiedTablesIt = new(
            new[] { typeof(GuestTableComponent) },
            new[] { typeof(GuestTableIsFreeTag) });
        _it.Init(_world);
        _occupiedTablesIt.Init(_world);
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

        foreach (var tableEntity in _occupiedTablesIt)
        {
            ref var packed = ref _workstationsAspect.GuestTablePool.Get(tableEntity).Guest;
            if (!packed.TryUnpack(out _, out var guestEntity))
            {
                Debug.Log("Стол потерял гостя");
                continue;
            }

            if (_guestAspect.GuestServicedTagPool.Has(guestEntity))
            {
                _guestAspect.GuestTableIsFreeTagPool.Add(tableEntity);
                Debug.Log("СВОБОДНАЯ КАССА");
            }
        }
    }
}