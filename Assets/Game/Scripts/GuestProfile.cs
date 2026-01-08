using UnityEngine;

namespace Game.Scripts
{
    [CreateAssetMenu(fileName = "GuestProfile", menuName = "Game/GuestProfile")]
    public class GuestProfile : ScriptableObject
    {
        public GameObject Guest;
        public int Cost; // Сколько "стоит" заспавнить (например, 5)
        public float Weight; // Вероятность выбора (если хватает денег)
    
        // Характеристики для компонента Guest
        public float PatienceSeconds; 
        public float MaxHunger;
        public float MoveSpeed;
        public int ReputationBlow;
    }
}