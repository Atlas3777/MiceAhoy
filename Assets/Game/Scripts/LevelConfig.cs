using System.Collections.Generic;
using UnityEngine;

namespace Game.Scripts.Systems
{
    [CreateAssetMenu(menuName = "Create LevelConfig", fileName = "Game/LevelConfig")]
    public class LevelConfig : ScriptableObject
    {
        public AnimationCurve DifficultyCurve;
        public float LevelDuration;
        public List<GuestProfile> AvailableGuests;
        
        public int BaseCreditWallet; // Стартовые деньги (чтобы сразу кто-то пришел)
        public float SpawnIntervalMin = 2f; // Чтобы не спавнить 10 врагов в одну миллисекунду
        public float SpawnIntervalMax = 5f;
    }
}
