using Game.Script.Aspects;
using Leopotam.EcsProto;
using Leopotam.EcsProto.QoL;
using Leopotam.EcsProto.Unity;
using UnityEngine;

public class GuestNavigateToDestroySystem : IProtoInitSystem, IProtoRunSystem
{
    private readonly Transform _exit;
    [DI] private ProtoWorld _world;
    [DI] private GuestAspect _guestAspect;

    private ProtoIt _leavingGroupsIterator;

    public GuestNavigateToDestroySystem(GameResources gameResources)
    {
        _exit = gameResources.GuestSpawner.transform;
    }

    public void Init(IProtoSystems systems)
    {
        _leavingGroupsIterator = new(new[]
        {
            typeof(GuestTag), typeof(GuestServicedTag),
        });
        _leavingGroupsIterator.Init(_world);
    }

    public void Run()
    {
        foreach (var guest in _leavingGroupsIterator)
        {
            ref var agent = ref _guestAspect.NavMeshAgentComponentPool.Get(guest).Agent;
                //Debug.LogError("ВЫХОДА НЕТ");
            agent.SetDestination(_exit.position);
            _guestAspect.GuestIsWalkingTagPool.GetOrAdd(guest);
        }
    }
}