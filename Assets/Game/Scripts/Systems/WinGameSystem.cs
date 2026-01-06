using System;
using Game.Scripts.Infrastructure;
using Leopotam.EcsProto;
using Leopotam.EcsProto.QoL;

namespace Game.Scripts.Systems
{
    public class WinGameSystem : IProtoInitSystem, IProtoRunSystem
    {
        [DI] ProtoWorld _world;
        
        public event Action Win;
        
        private readonly RuntimeLevelState _runtimeLevelState;

        public WinGameSystem(RuntimeLevelState runtimeLevelState)
        {
            
        }
        
        
        public void Init(IProtoSystems systems)
        {
            
        }

        public void Run()
        {
            
        }
    }
}