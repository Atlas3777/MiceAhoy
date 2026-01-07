using System;
using Game.Scripts.Aspects;
using Leopotam.EcsProto;
using Leopotam.EcsProto.QoL;
using UnityEngine;

namespace Game.Scripts.Systems
{
    public class PickPlaceSystem : IProtoInitSystem, IProtoRunSystem, IProtoDestroySystem
    {
        [DI] readonly WorkstationsAspect _workstationsAspect = default;
        [DI] readonly PlayerAspect _playerAspect = default;
        [DI] readonly BaseAspect _baseAspect = default;
        [DI] readonly ProtoWorld _world = default;

        private ProtoIt _iterator;

        public event Action PlayerPick;

        public void Init(IProtoSystems systems)
        {
            _iterator = new(new[] { typeof(PickPlaceEvent), typeof(HolderComponent) });
            _iterator.Init(_world);
        }

        public void Run()
        {
            foreach (var interactedEntity in _iterator)
            {
                ref var pickPlaceEvent = ref _workstationsAspect.PickPlaceEventPool.Get(interactedEntity);

                if (!pickPlaceEvent.Invoker.TryUnpack(out _, out var playerEntity)) continue;

                ref var interactedHolder = ref _baseAspect.HolderPool.Get(interactedEntity);
                ref var playerHolder = ref _baseAspect.HolderPool.Get(playerEntity);

                var playerHasItem = _playerAspect.HasItemTagPool.Has(playerEntity);
                var interactedHolderHasItem = _playerAspect.HasItemTagPool.Has(interactedEntity);
                ref var playerItem = ref _baseAspect.HolderPool.Get(playerEntity);

                switch (playerHasItem, tableHasItem: interactedHolderHasItem)
                {
                    case (false, false):
                        //Debug.Log("И на руках, и на столе пусто! (Ничего не делаем)");
                        break;

                    case (false, true):
                        PlayerPick?.Invoke();
                        if (!_workstationsAspect.ItemSourcePool.Has(interactedEntity))
                        {
                            //Debug.Log("Берем со стола!");
                            Helper.TransferItem(from: interactedEntity, to: playerEntity, ref interactedHolder,
                                ref playerHolder,
                                _playerAspect, _baseAspect);
                        }
                        // else
                        //     Debug.Log("Берём новый предмет");

                        _workstationsAspect.ItemPickEventPool.Add(interactedEntity);
                        break;

                    case (true, false):
                        //Debug.Log("Кладем на стол!");
                        _workstationsAspect.ItemPlaceEventPool.GetOrAdd(interactedEntity);

                        if (_workstationsAspect.ItemDestroyerComponentPool.Has(interactedEntity)
                            && playerItem.PickableItemGO)
                        {
                            Debug.Log("Убийство на улице вязов");
                            Helper.ReturnItemToGenerator(playerEntity, ref playerHolder, _playerAspect, _baseAspect);
                            continue;
                        }

                        Helper.TransferItem(from: playerEntity, to: interactedEntity, ref playerHolder,
                            ref interactedHolder,
                            _playerAspect, _baseAspect);
                        break;

                    case (true, true):
                        //Debug.Log("И в руках, и на столе что-то есть! (Свап или запрет)");
                        ref var tableItem = ref _baseAspect.HolderPool.Get(interactedEntity);

                        if (_workstationsAspect.ItemSourcePool.Has(interactedEntity)
                            && playerItem.Item == tableItem.Item
                            && !_workstationsAspect.GuestTablePool.Has(interactedEntity)
                            && playerItem.PickableItemGO)
                            Helper.ReturnItemToGenerator(playerEntity, ref playerHolder, _playerAspect, _baseAspect);
                        break;
                }
            }
        }

        public void Destroy()
        {
            //throw new System.NotImplementedException();
        }
    }
}