using Leopotam.EcsProto;
using UnityEngine;

namespace Game.Scripts.Systems
{
    public class GuestsGenerator : IProtoInitSystem
    {
        private readonly GameObject _guestPrefab;
        private ProtoWorld _world;
        public GuestsGenerator(GameObject guestPrefab)
        {
            this._guestPrefab = guestPrefab;
        }
        
        public void Init(IProtoSystems systems)
        {
            _world = systems.World();
        }
    }
}