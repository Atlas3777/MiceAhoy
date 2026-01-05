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
        
        private readonly LevelStateService _levelStateService;

        public WinGameSystem(LevelStateService levelStateService)
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