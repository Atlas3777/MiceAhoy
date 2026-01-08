using Game.Script.Aspects;
using Game.Scripts;
using Game.Scripts.Aspects;
using Game.Scripts.Infrastructure;
using Game.Scripts.Systems;
using Leopotam.EcsProto;
using Leopotam.EcsProto.QoL;
using UnityEngine;

public class AngryGuestLeaveSystem : IProtoInitSystem, IProtoRunSystem
{
    [DI] GuestAspect _guestAspect;
    [DI] WorkstationsAspect _workstationsAspect;
    [DI] BaseAspect _baseAspect;
    [DI] ProtoWorld _world;
    private ProtoIt _it;
    private ProtoItExc _occupiedTablesIt;

    private LevelState _state;
    public AngryGuestLeaveSystem(LevelState state)
    {
        _state =  state;
    }
    
    public void Init(IProtoSystems systems)
    {
        _it = new(new[] { typeof(WaitingOrderTag), typeof(TimerCompletedEvent),  typeof(GuestStateComponent) });
        
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

            var guest = guestEntity.Get<GuestStateComponent>();
            _baseAspect.ReputationRequestPool.NewEntity().Diff = guest.ReputationBlow;
            
            
            _guestAspect.WaitingOrderTagPool.Del(guestEntity);
            _guestAspect.GuestServedEventPool.GetOrAdd(guestEntity);
            _guestAspect.GuestServicedTagPool.GetOrAdd(guestEntity);
            _guestAspect.GuestIsWalkingTagPool.Add(guestEntity);
            _world.NewEntityWith<PlaySFXRequest>().SoundType = SoundType.AngryGuest;
        }

        foreach (var tableEntity in _occupiedTablesIt)
        {
            var guestEntity = _workstationsAspect.GuestTablePool.Get(tableEntity).Guest.GetEntity();

            if (_guestAspect.GuestServicedTagPool.Has(guestEntity))
            {
                _guestAspect.GuestTableIsFreeTagPool.Add(tableEntity);
                Debug.Log("СВОБОДНАЯ КАССА");
            }
        }
    }
}