using System;
using Game.Scripts.Aspects;
using Leopotam.EcsProto;
using Leopotam.EcsProto.QoL;

namespace Game.Scripts.Systems
{
    public class StartDaySystem : IProtoInitSystem, IProtoRunSystem 
    {
        [DI] ProtoWorld _world;
        [DI] private BaseAspect _baseAspect;
        
        private ProtoIt _it;
        
        public event Action OnStart;
        private bool _isStarted;
        
        public void Init(IProtoSystems systems)
        {
            _it = new(new []{typeof(InteractableComponent), typeof(DoorTag), typeof(PositionComponent),typeof(PickPlaceEvent) });
            _it.Init(_world);
        }

        public void Run()
        {
            foreach (var doorEntity in _it)
            {
                if(_isStarted) return;
                ref var r = ref doorEntity.Get<DoorTag>();
                r.doorStaticController.Open();
                ref var rr = ref doorEntity.Get<InteractableComponent>();
                rr.Outline.SetActive(false);
                rr.IsActive =  false;
                doorEntity.Del<InteractableComponent>();
                _isStarted = true;
                OnStart?.Invoke();
            }
        }
    }
}