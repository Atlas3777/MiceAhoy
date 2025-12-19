using Game.Script.Systems;
using UnityEngine;

namespace Game.Script.Factories
{
    public class GuestsGeneratorFactory
    {
        private readonly GameObject _guestPrefab;
        public GuestsGeneratorFactory(GameObject guestPrefab) => _guestPrefab = guestPrefab;
        
        public GuestsGenerator CreateProtoSystem() => new(_guestPrefab);
    }
}