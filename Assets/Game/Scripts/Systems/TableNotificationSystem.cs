using System;
using System.Collections.Generic;
using Game.Script.Aspects;
using Game.Scripts.Aspects;
using Leopotam.EcsProto;
using Leopotam.EcsProto.QoL;
using UnityEngine;

namespace Game.Script.Systems
{
    public class TableNotificationSystem : IProtoInitSystem, IProtoRunSystem
    {
        [DI] private WorkstationsAspect _workstationsAspect;
        [DI] private PlayerAspect _playerAspect;
        [DI] private GuestAspect _guestAspect;
        [DI] private GuestGroupAspect _guestGroupAspect;
        [DI] private BaseAspect _baseAspect;
        private ProtoIt _tablesIterator;

        public event Action GuestGroupServedTagPool;

        
        public void Init(IProtoSystems systems)
        {
            var world = systems.World();
            _tablesIterator = new(new[]
            {
                typeof(GuestTableComponent), typeof(ItemPlaceEvent), typeof(HolderComponent),
                typeof(PositionComponent), typeof(InteractableComponent)
            });
            _tablesIterator.Init(world);
        }

        public void Run()
        {
            foreach (var tableEntity in _tablesIterator)
            {
                ref var tableComponent = ref _workstationsAspect.GuestTablePool.Get(tableEntity);
                if (!tableComponent.GuestGroup.TryUnpack(out _, out var guestGroupEntity))
                {
                    Debug.LogWarning("Не получилось извлечь группу!");
                    continue;
                }
                ref var holder = ref _playerAspect.HolderPool.Get(tableEntity);
                var group = _guestGroupAspect.GuestGroupPool.Get(guestGroupEntity);
                if (!_guestGroupAspect.WaitingOrderTagPool.Has(guestGroupEntity))
                {
                    Debug.Log("Пока не пришли, не едим");
                    continue;
                }
                var packedGuests = group.includedGuests;

                foreach (var packedGuest in packedGuests)
                    if (packedGuest.TryUnpack(out _, out var guestEntity))
                    {
                        if (TryServiceGuest(guestEntity, tableEntity, ref holder))
                        {
                            Debug.Log("Гость хавает");
                            break;
                        }
                    }
                
                var isEverybodyServed = true;
                foreach (var packedGuest in packedGuests)
                    if (packedGuest.TryUnpack(out _, out var guestEntity))
                    {
                        if (!_guestAspect.GuestServicedPool.Has(guestEntity))
                        {
                            isEverybodyServed = false;
                            break;
                        }
                    }

                if (isEverybodyServed)
                {
                    Debug.Log("Гости поели, щяс уйдут");
                    
                    _guestGroupAspect.WaitingOrderTagPool.Del(guestGroupEntity);
                    _guestGroupAspect.GuestGroupServedEventPool.Add(guestGroupEntity);
                    _guestGroupAspect.GuestGroupServedTagPool.Add(guestGroupEntity);
                    _baseAspect.TimerCompletedPool.Add(guestGroupEntity);
                    _workstationsAspect.ItemGenerationAvailablePool.Add(tableEntity);
                    _playerAspect.HasItemTagPool.Add(tableEntity);
                }
            }
        }
        
        private bool TryServiceGuest(ProtoEntity guestEntity, ProtoEntity tableEntity, ref HolderComponent holder)
        {
            ref var wantedItem = ref _guestAspect.WantedItemPool.Get(guestEntity).WantedItem;

            if (!wantedItem.Is(holder.Item))
                return false;

            if (_guestAspect.GuestServicedPool.Has(guestEntity))
                return false;
            
            ref var wantedItemVisualization = ref _guestAspect.WantedItemVisualizationPool.Get(guestEntity);
            wantedItemVisualization.Visualization.SetActive(false);
            _guestAspect.GuestServicedPool.Add(guestEntity);
            
            Helper.EatItem(tableEntity, ref holder, _playerAspect);
            
            Debug.Log("WINWINWIN");
            return true;
        }
    }
}