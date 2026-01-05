using Game.Scripts.Aspects;
using Leopotam.EcsProto;
using Leopotam.EcsProto.QoL;

namespace Game.Scripts.Systems
{
    public class ReputationSystem : IProtoInitSystem, IProtoRunSystem
    {
        private ProtoIt _it;
        [DI]private ProtoWorld _world;
        
        public void Init(IProtoSystems systems)
        {
            _it = new(new[] { typeof(ReputationRequest) });
            _it.Init(_world);
        }

        public void Run()
        {
            foreach (var VARIABLE in _it)
            {
                
            }
        }
    }
}