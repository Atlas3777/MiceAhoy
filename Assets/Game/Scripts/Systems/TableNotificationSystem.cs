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
                ref var holder = ref _baseAspect.HolderPool.Get(tableEntity);
                if (!tableComponent.Guest.TryUnpack(out _, out var guestEntity))
                {
                    Debug.LogWarning("Не получилось извлечь гостя!");
                    continue;
                }
                if (!_guestAspect.WaitingOrderTagPool.Has(guestEntity))
                {
                    Debug.Log("Пока не пришли, не едим");
                    continue;
                }
                if (!IsGuestFull(guestEntity, tableEntity, ref holder))
                {
                    Debug.Log("Гость хавает");
                    continue;
                }
                Debug.Log("Гость наелся, уходит");
                
                _guestAspect.WaitingOrderTagPool.Del(guestEntity);
                _guestAspect.GuestServedEventPool.Add(guestEntity);
                _guestAspect.GuestServicedTagPool.Add(guestEntity);
                _baseAspect.TimerCompletedPool.Add(guestEntity);
                _workstationsAspect.ItemGenerationAvailablePool.Add(tableEntity);
            }
        }
        
        private bool IsGuestFull(ProtoEntity guestEntity, ProtoEntity tableEntity, ref HolderComponent holder)
        {
            ref var hunger = ref _guestAspect.GuestStateComponentPool.Get(guestEntity).Hunger;
            ref var satietyRestoration = ref _baseAspect.HolderPool.Get(tableEntity)
                .PickableItemInfo.GetComponent<PickableItemInfoWrapper>().satietyRestoration;
            
            hunger -= satietyRestoration;
            
            Helper.EatItem(tableEntity, ref holder, _playerAspect);
            
            Debug.Log(hunger);
            return hunger <= 0;
        }
    }
}