using Game.Script.Aspects;
using Leopotam.EcsProto;
using Leopotam.EcsProto.QoL;
using UnityEngine;

namespace Game.Scripts.Systems
{
    public class GuestNavigateToTableSystem : IProtoInitSystem, IProtoRunSystem
    {
        [DI] private WorkstationsAspect _workstationsAspect;
        [DI] private GuestAspect _guestAspect;
        [DI] private ProtoWorld _world;
        private ProtoIt _guestsWithTablesIterator;
        
        public void Init(IProtoSystems systems)
        {
            _guestsWithTablesIterator = new(new[]
            {
                typeof(GuestTag), typeof(GotTableEvent)
            });
            _guestsWithTablesIterator.Init(_world);
        }

        public void Run()
        {
            foreach (var guestEntity in _guestsWithTablesIterator)
            {
                if (!_guestAspect.TargetPositionComponentPool.Get(guestEntity).Table.TryUnpack(out _, out var table))
                {
                    Debug.LogWarning("Стол куда-то исчез...");
                    continue;
                }

                ref var tableComponent = ref _workstationsAspect.GuestTablePool.Get(table);
                
                ref var agent = ref _guestAspect.NavMeshAgentComponentPool.Get(guestEntity).Agent;
                agent.SetDestination(tableComponent.GuestPosition.position);
                _guestAspect.GuestIsWalkingTagPool.GetOrAdd(guestEntity);

                _guestAspect.GotTableEventPool.Del(guestEntity);
            }
        }
    }
}