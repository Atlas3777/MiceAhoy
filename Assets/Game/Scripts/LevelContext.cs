using UnityEngine;
using UnityEngine.Serialization;

namespace Game.Scripts
{
    public class LevelContext : MonoBehaviour
    {
        [FormerlySerializedAs("spawnRegistry")] public PositionsRegistry positionsRegistry;
        public Transform levelCenter;
    }
}