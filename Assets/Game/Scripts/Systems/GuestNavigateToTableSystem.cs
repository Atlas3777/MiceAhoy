using Game.Script.Aspects;
using Leopotam.EcsProto;
using Leopotam.EcsProto.QoL;
using UnityEngine;

namespace Game.Script.Systems
{
    public class GuestNavigateToTableSystem : IProtoInitSystem, IProtoRunSystem
    {
        [DI] private GuestGroupAspect _guestGroupAspect;
        [DI] private WorkstationsAspect _workstationsAspect;
        [DI] private GuestAspect _guestAspect;
        [DI] private ProtoWorld _world;
        private ProtoIt _groupsWithTablesIterator;
        
        public void Init(IProtoSystems systems)
        {
            _groupsWithTablesIterator = new(new[]
            {
                typeof(GuestGroupTag), typeof(GroupGotTableEvent)
            });
            _groupsWithTablesIterator.Init(_world);
        }

        public void Run()
        {
            foreach (var groupEntity in _groupsWithTablesIterator)
            {
                ref var packedGuests = ref _guestGroupAspect.GuestGroupPool
                    .Get(groupEntity).includedGuests;
                if (!_guestGroupAspect.GuestGroupPool.Get(groupEntity).table.TryUnpack(out _, out var table))
                {
                    Debug.LogWarning("Стол куда-то исчез...");
                    continue;
                }

                ref var tableComponent = ref _workstationsAspect.GuestTablePool.Get(table);
                Debug.Log("Распределяем места за столом...");

                var i = 0;
                foreach (var guestEntity in packedGuests)
                {
                    if (!guestEntity.TryUnpack(out _, out var guest))
                    {
                        Debug.LogWarning("Гость не распакован!!");
                    }
                        
                    ref var agent = ref _guestAspect.NavMeshAgentComponentPool.Get(guest).Agent;
                    agent.SetDestination(tableComponent.guestPlaces[i]);
                    _guestAspect.GuestIsWalkingTagPool.Add(guest);
                    ++i;
                }

                _guestGroupAspect.GroupGotTableEventPool.Del(groupEntity);
            }
        }
    }
}