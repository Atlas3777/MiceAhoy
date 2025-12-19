using Leopotam.EcsProto;
using Leopotam.EcsProto.QoL;
using UnityEngine;

namespace Game.Script.Systems
{
    public class DirtyPlatePickupSystem : IProtoInitSystem, IProtoRunSystem
    {
        [DI] private WorkstationsAspect _workstationsAspect;
        [DI] readonly PlayerAspect _playerAspect;
        
        private ProtoIt _tablesWithPlatesIterator;
        
        public void Init(IProtoSystems systems)
        {
            var world = systems.World();
            _tablesWithPlatesIterator = new(new[]
            {
                typeof(PlatesOnWorkstationComponent), typeof(ItemPickEvent), typeof(ItemSourceComponent)
            });
            _tablesWithPlatesIterator.Init(world);
        }

        public void Run()
        {
            foreach (var tableEntity in _tablesWithPlatesIterator)
            {
                ref var remainPlates = ref _workstationsAspect.PlatesOnTablePool.Get(tableEntity);
                --remainPlates.PlatesOnTable;
                Debug.Log($"осталось {remainPlates.PlatesOnTable} тарелок");
                if (remainPlates.PlatesOnTable == 0)
                {
                    _workstationsAspect.PlatesOnTablePool.Del(tableEntity);
                    _workstationsAspect.ItemGenerationAvailablePool.Del(tableEntity);
                    _playerAspect.HolderPool.Get(tableEntity).Clear();
                }
            }
        }
    }
}