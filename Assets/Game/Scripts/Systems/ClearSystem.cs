using Game.Script.Aspects;
using Game.Scripts.Aspects;
using Leopotam.EcsProto;
using Leopotam.EcsProto.QoL;

namespace Game.Scripts.Systems
{
    public class ClearSystem : IProtoInitSystem, IProtoRunSystem
    {
        [DI] readonly ProtoWorld _world;
        [DI] readonly WorkstationsAspect _workstationsAspect;
        [DI] readonly BaseAspect _baseAspect;
        [DI] readonly GuestAspect _guestAspect;
        [DI] readonly PlayerAspect _playerAspect;

        private ProtoIt _iteratorPick;
        private ProtoIt _iteratorPlace;
        private ProtoIt _iteratorPickPlace;
        private ProtoIt _iteratorTimer;
        private ProtoIt _placeWorkstationIt;
        private ProtoIt _interactedEventIt;
        private ProtoIt _reachedTargetPositionEventIt;
        private ProtoIt _guestGroupServedEventIt;
        private ProtoIt _playerInitializeItEventIt;
        private ProtoIt _selectedByPlayerTagIt;
        private ProtoIt _updateQueueEventIt;
        private ProtoIt _guestLeavingQueueEventIt;
        private ProtoIt _guestEnteringQueueEventIt;

        public void Init(IProtoSystems systems)
        {
            _iteratorPick = new(new[] { typeof(ItemPickEvent) });
            _iteratorPlace = new(new[] { typeof(ItemPlaceEvent) });
            _iteratorPickPlace = new(new[] { typeof(PickPlaceEvent) });
            _iteratorTimer = new(new[] { typeof(TimerComponent), typeof(TimerCompletedEvent) });
            _placeWorkstationIt = new(new[] { typeof(PlaceWorkstationEvent) });
            _interactedEventIt = new(new[] { typeof(InteractedEvent) });
            _reachedTargetPositionEventIt = new(new[] { typeof(ReachedTargetPositionEvent) });
            _guestGroupServedEventIt = new(new[] { typeof(GuestServedEvent) });
            _playerInitializeItEventIt = new(new[] { typeof(PlayerInitializeEvent) });
            _selectedByPlayerTagIt = new(new[] { typeof(SelectedByPlayerEvent)});
            _updateQueueEventIt = new(new[] { typeof(UpdateQueueEvent) });
            _guestLeavingQueueEventIt = new(new[] { typeof(GuestLeavingQueueEvent) });
            _guestEnteringQueueEventIt = new(new[] { typeof(GuestEnteringQueueEvent) });

            _iteratorPick.Init(_world);
            _iteratorPlace.Init(_world);
            _iteratorPickPlace.Init(_world);
            _iteratorTimer.Init(_world);
            _placeWorkstationIt.Init(_world);
            _interactedEventIt.Init(_world);
            _reachedTargetPositionEventIt.Init(_world);
            _guestGroupServedEventIt.Init(_world);
            _playerInitializeItEventIt.Init(_world);
            _selectedByPlayerTagIt.Init(_world);
            _updateQueueEventIt.Init(_world);
            _guestLeavingQueueEventIt.Init(_world);
            _guestEnteringQueueEventIt.Init(_world);
        }

        public void Run()
        {
            foreach (var item in _iteratorPick)
                _workstationsAspect.ItemPickEventPool.Del(item);

            foreach (var item in _iteratorPlace)
                _workstationsAspect.ItemPlaceEventPool.Del(item);

            foreach (var item in _iteratorPickPlace)
                _workstationsAspect.PickPlaceEventPool.Del(item);

            foreach (var item in _iteratorTimer)
            {
                _baseAspect.TimerCompletedPool.Del(item);
                _baseAspect.TimerPool.Del(item);
            }

            foreach (var item in _placeWorkstationIt)
                _workstationsAspect.PlaceWorkstationEventPool.Del(item);

            foreach (var item in _interactedEventIt)
                _workstationsAspect.InteractedEventPool.Del(item);

            foreach (var item in _reachedTargetPositionEventIt)
                _guestAspect.ReachedTargetPositionEventPool.Del(item);

            foreach (var item in _guestGroupServedEventIt)
                _guestAspect.GuestServedEventPool.Del(item);

            foreach (var item in _playerInitializeItEventIt)
                _playerAspect.PlayerInitializeEventPool.Del(item);

            foreach (var item in _selectedByPlayerTagIt)
                _baseAspect.SelectedByPlayerTagPool.Del(item);
            
            foreach (var item in _updateQueueEventIt)
                _guestAspect.UpdateQueueEventPool.Del(item);
            
            foreach (var item in _guestLeavingQueueEventIt)
                _guestAspect.GuestLeavingQueueEventPool.Del(item);
            
            foreach (var item in _guestEnteringQueueEventIt)
                _guestAspect.GuestEnteringQueueEventPool.Del(item);
        }
    }
}