using Game.Script.Aspects;
using Leopotam.EcsProto;
using Leopotam.EcsProto.QoL;
using Leopotam.EcsProto.Unity;
using UnityEngine;

public class GuestNavigateToDestroySystem : IProtoInitSystem, IProtoRunSystem
{
    //[DIUnity("Exit")] private readonly Transform _exitTransform = default;
    [DI] private ProtoWorld _world;
    [DI] private GuestAspect _guestAspect;
    [DI] private GuestGroupAspect _guestGroupAspect;

    private ProtoItExc _leavingGroupsIterator;

    public void Init(IProtoSystems systems)
    {
        _leavingGroupsIterator = new(new[]
        {
            typeof(GuestGroupTag), typeof(GuestGroupServedEvent)
        }, new[]
        {
            typeof(WaitingOrderTag), typeof(WaitingTakeOrderTag)
        });
        _leavingGroupsIterator.Init(_world);
    }

    public void Run()
    {
        foreach (var leavingGroupEntity in _leavingGroupsIterator)
        {
            ref var packedGuests = ref _guestGroupAspect.GuestGroupPool
                .Get(leavingGroupEntity).includedGuests;
            foreach (var packedGuestEntity in packedGuests)
            {
                if (!packedGuestEntity.TryUnpack(out _, out var guest))
                {
                    Debug.LogWarning("Гость опять не распаковался тварь");
                    continue;
                }

                ref var agent = ref _guestAspect.NavMeshAgentComponentPool.Get(guest).Agent;
                Debug.LogError("ВЫХОДА НЕТ");
                //agent.SetDestination(_exitTransform.position);
                _guestAspect.GuestIsWalkingTagPool.Add(guest);
            }

            _guestGroupAspect.GroupIsWalkingPool.Add(leavingGroupEntity);
            Debug.Log("Группа получила координаты выхода, уходит");
        }
    }
}