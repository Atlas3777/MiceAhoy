using Unity.AI.Navigation;
using UnityEngine;

namespace Game.Scripts
{
    public class LevelContext : MonoBehaviour
    { 
        public PositionsRegistry positionsRegistry;
        public Transform levelCenter;
        public Transform enemyPreview;
        public BoxCollider enemyBox;
        public NavMeshSurface navMeshSurface;
    }
}