using System;
using UnityEngine;

namespace Game.Scripts
{
    [Serializable]
    public class SpawnRegistry
    {
        public Transform PlayerSpawn; 
        public Transform GuestSpawn; 
        public Transform GuestDestroy; 
    }
}