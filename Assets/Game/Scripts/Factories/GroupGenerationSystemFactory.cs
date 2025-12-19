using Game.Script.Systems;
using UnityEngine;

namespace Game.Script.Factories
{
    public class GroupGenerationSystemFactory
    {
        private readonly GameResources _gameResources;
        public GroupGenerationSystemFactory(GameResources gameResources) =>
            _gameResources = gameResources;
        
        public GroupGenerationSystem CreateProtoSystem() => new(_gameResources.Guest.gameObject);
    }
}