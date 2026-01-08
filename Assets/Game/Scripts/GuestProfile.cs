using UnityEngine;

namespace Game.Scripts
{
    [CreateAssetMenu(fileName = "GuestProfile", menuName = "Game/GuestProfile")]
    public class GuestProfile : ScriptableObject
    {
        public GameObject Guest;
        public int Cost; 
        public float Weight;
    
        // Характеристики для компонента Guest
        public float PatienceSeconds; 
        public float MaxHunger;
        public float MoveSpeed;
        public int ReputationLoss;
    }
}