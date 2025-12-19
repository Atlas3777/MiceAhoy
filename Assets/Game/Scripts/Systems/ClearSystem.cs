using Game.Script.Aspects;
using Leopotam.EcsProto;
using Leopotam.EcsProto.QoL;
using UnityEngine;

namespace Game.Script.Systems
{
    public class ClearSystem : IProtoInitSystem, IProtoRunSystem
    {
        [DI] readonly ProtoWorld _world;
        [DI] readonly WorkstationsAspect _workstationsAspect;
        [DI] readonly BaseAspect _baseAspect;
        [DI] readonly GuestAspect _guestAspect;
        [DI] readonly GuestGroupAspect _guestGroupAspect;
        [DI] readonly PlayerAspect _playerAspect;

        private ProtoIt _iteratorPick;
        private ProtoIt _iteratorPlace;
        private ProtoIt _iteratorPickPlace;
        private ProtoIt _iteratorTimer;
        private ProtoIt _placeWorkstationIt;
        private ProtoIt _interactedEventIt;
        private ProtoIt _reachedTargetPositionEventIt;
        private ProtoIt _groupArrivedEventIt;
        private ProtoIt _guestGroupServedEventIt;
        private ProtoIt _playerInitializeItEventIt;

        public void Init(IProtoSystems systems)
        {
            _iteratorPick = new(new[] { typeof(ItemPickEvent) });
            _iteratorPlace = new(new[] { typeof(ItemPlaceEvent) });
            _iteratorPickPlace = new(new[] { typeof(PickPlaceEvent) });
            _iteratorTimer = new(new[] { typeof(TimerComponent), typeof(TimerCompletedEvent) });
            _placeWorkstationIt = new(new[] { typeof(PlaceWorkstationEvent) });
            _interactedEventIt = new(new[] { typeof(InteractedEvent) });
            _reachedTargetPositionEventIt = new (new[] {typeof(ReachedTargetPositionEvent) });
            _groupArrivedEventIt = new (new[] {typeof(GroupArrivedEvent) });
            _guestGroupServedEventIt = new (new[] {typeof(GuestGroupServedEvent) });
            _playerInitializeItEventIt = new  (new[] {typeof(PlayerInitializeEvent) });
            
            _iteratorPick.Init(_world);
            _iteratorPlace.Init(_world);
            _iteratorPickPlace.Init(_world);
            _iteratorTimer.Init(_world);
            _placeWorkstationIt.Init(_world);
            _interactedEventIt.Init(_world);
            _reachedTargetPositionEventIt.Init(_world);
            _groupArrivedEventIt.Init(_world);
            _guestGroupServedEventIt.Init(_world);
            _playerInitializeItEventIt.Init(_world);
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
            
            foreach(var item in _interactedEventIt)
                _workstationsAspect.InteractedEventPool.Del(item);
            
            foreach(var item in _reachedTargetPositionEventIt)
                _guestAspect.ReachedTargetPositionEventPool.Del(item);
            
            foreach (var item in _groupArrivedEventIt)
                _guestGroupAspect.GroupArrivedEventPool.Del(item);
            
            foreach (var item in _guestGroupServedEventIt)
                _guestGroupAspect.GuestGroupServedEventPool.Del(item);
            
            foreach(var item in _playerInitializeItEventIt)
                _playerAspect.PlayerInitializeEventPool.Del(item);
        }
    }
}