using System.Collections.Generic;
using Leopotam.EcsProto;
using Leopotam.EcsProto.QoL;
using UnityEngine;

namespace Game.Script.Systems
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