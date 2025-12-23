using Game.Script.Aspects;
using Leopotam.EcsProto;
using Leopotam.EcsProto.QoL;
using UnityEngine;

namespace Game.Script.Systems
{
    public class PickPlaceSystem : IProtoInitSystem, IProtoRunSystem, IProtoDestroySystem
    {
        [DI] readonly WorkstationsAspect _workstationsAspect = default;
        [DI] readonly GuestAspect _guestAspect = default;
        [DI] readonly PlayerAspect _playerAspect = default;
        [DI] readonly BaseAspect _baseAspect = default;
        [DI] readonly ProtoWorld _world = default;

        private ProtoIt _iterator;

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

                // 1. Проверяем валидность игрока. Если игрок "мертв", пропускаем итерацию.
                if (!pickPlaceEvent.Invoker.TryUnpack(out _, out var playerEntity))
                {
                    _workstationsAspect.PickPlaceEventPool.DelIfExists(interactedEntity);
                    continue;
                }

                ref var interactedHolder = ref _playerAspect.HolderPool.Get(interactedEntity);
                ref var playerHolder = ref _playerAspect.HolderPool.Get(playerEntity);

                // 2. Определяем состояние через наличие тегов (или можно проверять Holder.Entity.IsAlive())
                bool playerHasItem = _playerAspect.HasItemTagPool.Has(playerEntity);
                bool interactedHolderHasItem = _playerAspect.HasItemTagPool.Has(interactedEntity);

                // 3. Используем switch по кортежу для обработки 4-х ситуаций
                switch (playerHasItem, tableHasItem: interactedHolderHasItem)
                {
                    case (false, false):
                        Debug.Log("И на руках, и на столе пусто! (Ничего не делаем)");
                        break;

                    case (false, true):
                        if (!_workstationsAspect.ItemSourcePool.Has(interactedEntity))
                        {
                            Debug.Log("Берем со стола!");
                            Helper.TransferItem(from: interactedEntity, to: playerEntity, ref interactedHolder,
                                ref playerHolder,
                                _playerAspect, _baseAspect);
                        }
                        else
                            Debug.Log("Берём новый предмет");
                        _workstationsAspect.ItemPickEventPool.Add(interactedEntity);
                        break;

                    case (true, false):
                        Debug.Log("Кладем на стол!");
                        _workstationsAspect.ItemPlaceEventPool.GetOrAdd(interactedEntity);
                        Helper.TransferItem(from: playerEntity, to: interactedEntity, ref playerHolder, ref interactedHolder,
                            _playerAspect, _baseAspect);
                        break;

                    case (true, true):
                        Debug.Log("И в руках, и на столе что-то есть! (Свап или запрет)");
                        ref var playerItem = ref _playerAspect.HolderPool.Get(playerEntity);
                        ref var tableItem = ref _playerAspect.HolderPool.Get(interactedEntity);

                        if (_workstationsAspect.ItemSourcePool.Has(interactedEntity)
                            && playerItem.Item == tableItem.Item
                            && !_workstationsAspect.GuestTablePool.Has(interactedEntity)
                            && playerItem.PickableItemVisual)
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