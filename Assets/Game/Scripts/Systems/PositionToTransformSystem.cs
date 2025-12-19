using Leopotam.EcsProto;
using Leopotam.EcsProto.QoL;

namespace Game.Script.Systems
{
    public class PositionToTransformSystem : IProtoInitSystem, IProtoRunSystem
    {
        [DI] private PhysicsAspect _physicsAspect;
    
        private ProtoWorld _world;
        private ProtoIt _iterator;

        public void Init(IProtoSystems systems)
        {
            _world = systems.World();
            _iterator = new ProtoIt(new[]
            {
                typeof(PositionComponent),
                typeof(UnityTransformRef)
            });

            _iterator.Init(_world);
        }

        public void Run()
        {
            foreach (var entity in _iterator)
            {
                ref var pos = ref _physicsAspect.PositionPool.Get(entity);
                ref var tr = ref _physicsAspect.UnityTransformPool.Get(entity);
            
                tr.Transform.position = pos.Position;
            }
        }
    }
}