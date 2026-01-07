using Game.Scripts.Aspects;
using Leopotam.EcsProto;
using Leopotam.EcsProto.QoL;

namespace Game.Scripts.Systems
{
    public class OutlineSystem : IProtoInitSystem, IProtoRunSystem
    {
        [DI] ProtoWorld _world;
        [DI] BaseAspect _baseAspect;

        private ProtoIt _it;
        private ProtoItExc _itDel;

        public void Init(IProtoSystems systems)
        {
            _it = new(new[] { typeof(InteractableComponent), typeof(SelectedByPlayerEvent) });
            _itDel = new ProtoItExc(new[] { typeof(InteractableComponent) }, new[] { typeof(SelectedByPlayerEvent) });

            _it.Init(_world);
            _itDel.Init(_world);
        }

        public void Run()
        {
            foreach (var outlineEntity in _it)
            {
                ref var interactableComponent = ref _baseAspect.InteractableComponentPool.Get(outlineEntity);
                if (interactableComponent.IsActive) continue;

                interactableComponent.IsActive = true;
                interactableComponent.Outline.SetActive(true);
            }

            foreach (var outlineEntity in _itDel)
            {
                ref var interactableComponent = ref _baseAspect.InteractableComponentPool.Get(outlineEntity);

                interactableComponent.IsActive = false;
                interactableComponent.Outline.SetActive(false);
            }
        }
    }
}