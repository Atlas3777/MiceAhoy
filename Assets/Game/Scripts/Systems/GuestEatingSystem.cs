using System.Globalization;
using Game.Script.Aspects;
using Game.Scripts.Aspects;
using Leopotam.EcsProto;
using Leopotam.EcsProto.QoL;
using UnityEngine;

namespace Game.Scripts.Systems
{
    public class GuestEatingSystem : IProtoInitSystem, IProtoRunSystem
    {
        [DI] private WorkstationsAspect _workstationsAspect;
        [DI] private PlayerAspect _playerAspect;
        [DI] private GuestAspect _guestAspect;
        [DI] private BaseAspect _baseAspect;

        private readonly PickableService _pickableService;
        private ProtoIt _tablesWithFoodIt;

        public GuestEatingSystem(PickableService pickableService)
        {
            _pickableService = pickableService;
        }

        public void Init(IProtoSystems systems)
        {
            _tablesWithFoodIt = new(new[]
            {
                typeof(GuestTableComponent),
                typeof(ItemPlaceEvent),
                typeof(HolderComponent),
                typeof(PositionComponent),
                typeof(InteractableComponent)
            });
            _tablesWithFoodIt.Init(systems.World());
        }

        public void Run()
        {
            foreach (var tableEntity in _tablesWithFoodIt)
            {
                ref var tableComponent = ref _workstationsAspect.GuestTablePool.Get(tableEntity);

                if (!tableComponent.Guest.TryUnpack(out _, out var guestEntity))
                {
                    continue;
                }

                if (!_guestAspect.WaitingOrderTagPool.Has(guestEntity))
                {
                    Debug.Log("Гость не ждет заказ, не едим");
                    continue;
                }

                if (!TryConsumeFood(guestEntity, tableEntity))
                    _baseAspect.TimerPool.Get(guestEntity).Completed = true;
            }
        }

        private bool TryConsumeFood(ProtoEntity guestEntity, ProtoEntity tableEntity)
        {
            ref var holder = ref _baseAspect.HolderPool.Get(tableEntity);
            var eatType = holder.Item;

            if (_pickableService.TryGetPickable(eatType, out var item))
            {
                ref var guestState = ref _guestAspect.GuestStateComponentPool.Get(guestEntity);

                guestState.Hunger -= item.satietyRestoration;

                ref var view = ref guestEntity.Get<GuestViewComponent>();
                if (view.CurrentHunger != null)
                {
                    view.CurrentHunger.text = guestState.Hunger.ToString(CultureInfo.InvariantCulture);
                    view.HungerBarImage.fillAmount = guestState.Hunger / guestState.MaxHunger;
                }

                Debug.Log($"Гость поел. Текущий голод: {guestState.Hunger}");

                Helper.EatItem(tableEntity, ref holder, _playerAspect);
                
                if (guestState.Hunger <= 0)
                {
                    guestState.Hunger = 0;
                    _guestAspect.GuestTableIsFreeTagPool.Add(tableEntity);
                }
                
                if (eatType == typeof(Trash))
                {
                    Debug.Log("Сам свои угольки хавай");
                    return false;
                }
            }

            return true;
        }
    }
}