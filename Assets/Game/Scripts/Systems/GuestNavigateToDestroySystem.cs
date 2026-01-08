using Game.Script.Aspects;
using Game.Scripts;
using Leopotam.EcsProto;
using Leopotam.EcsProto.QoL;
using UnityEngine;

public class GuestNavigateToDestroySystem : IProtoInitSystem, IProtoRunSystem
{
    private readonly Transform _exit;
    [DI] private ProtoWorld _world;
    [DI] private GuestAspect _guestAspect;

    private ProtoIt _leavingGroupsIterator;

    public GuestNavigateToDestroySystem(PositionsRegistry positionsRegistry)
    {
        _exit = positionsRegistry.GuestDestroy;
    }

    public void Init(IProtoSystems systems)
    {
        _leavingGroupsIterator = new(new[] { typeof(GuestTag), typeof(GuestServicedTag), });
        _leavingGroupsIterator.Init(_world);
    }

    public void Run()
    {
        foreach (var guest in _leavingGroupsIterator)
        {
            ref var agent = ref _guestAspect.NavMeshAgentComponentPool.Get(guest).Agent;
            agent.SetDestination(_exit.position);
            _guestAspect.GuestIsWalkingTagPool.GetOrAdd(guest);
        }
    }
}