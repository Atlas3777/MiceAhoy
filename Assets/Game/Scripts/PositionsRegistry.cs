using System;
using UnityEngine;

namespace Game.Scripts
{
    [Serializable]
    public class PositionsRegistry
    {
        public Transform PlayerSpawn; 
        public Transform GuestSpawn; 
        public Transform GuestDestroy;
        public Transform GuestsQueueHead;
    }
}