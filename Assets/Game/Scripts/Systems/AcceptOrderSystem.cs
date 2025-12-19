using System.Collections.Generic;
using System.Linq;
using Game.Script.Aspects;
using Leopotam.EcsProto;
using Leopotam.EcsProto.QoL;
using UnityEngine;

namespace Game.Script.Systems
{
    public class AcceptOrderSystem : IProtoInitSystem, IProtoRunSystem
    {
        [DI] private WorkstationsAspect _workstationsAspect;
        [DI] private GuestAspect _guestAspect;
        [DI] private GuestGroupAspect _guestGroupAspect;
        private ProtoWorld _world;
        
        private ProtoItExc _interactedTableIterator;
        private ProtoIt _groupsIterator;
        
        public void Init(IProtoSystems systems)
        {
            _world = systems.World();

            _interactedTableIterator = new(new[]
            {
                typeof(GuestTableComponent), typeof(PickPlaceEvent),
                typeof(PositionComponent), typeof(InteractableComponent)
            },
            new[]
            {
                typeof(ItemPickEvent), typeof(ItemPlaceEvent)
            });
            _groupsIterator = new(new[]
            {
                typeof(GuestGroupTag), typeof(WaitingTakeOrderTag)
            });

            _interactedTableIterator.Init(_world);
            _groupsIterator.Init(_world);
        }

        public void Run()
        {
            foreach (var tableEntity in _interactedTableIterator)
            {
                Debug.Log("Пытаемся взять заказ");
                ref var tableComponent = ref _workstationsAspect.GuestTablePool.Get(tableEntity);
                if (!tableComponent.GuestGroup.TryUnpack(out _, out var guestGroupEntity))
                {
                    Debug.LogWarning("Не получилось извлечь группу!");
                    continue;
                }
                
                var groupIsAllowedToTakeOrder = false;
                foreach (var allowedGroup in _groupsIterator)
                {
                    if (allowedGroup == guestGroupEntity)
                    {
                        groupIsAllowedToTakeOrder = true;
                        break;
                    }
                }
                if (!groupIsAllowedToTakeOrder)
                {
                    Debug.Log("Группа за столом ещё не готова для принятия заказа");
                    continue;
                }
                
                var packedGuests = _guestGroupAspect.GuestGroupPool
                    .Get(guestGroupEntity).includedGuests;

                var isSucessfullyOrderTaken = true;

                foreach (var packedGuest in packedGuests)
                {
                    if (packedGuest.TryUnpack(out _, out var guestEntity))
                    {
                        Debug.Log("Получен заказ от гостя");
                        _workstationsAspect.InteractedEventPool.Add(guestEntity);
                        _guestAspect.WantedItemVisualizationPool.Get(guestEntity).Visualization.SetActive(true);
                    }
                    else
                    {
                        Debug.LogWarning("Не получилось обработать заказ гостя");
                        isSucessfullyOrderTaken = false;
                        break;
                    }
                }

                if (isSucessfullyOrderTaken)
                {
                    Debug.Log("Заказ успешно принят у всей группы");
                    _workstationsAspect.InteractedEventPool.Add(guestGroupEntity);
                }
            }
        }
    }
}
