using System.Collections.Generic;
using Game.Scripts.Infrastructure;
using Game.Scripts.LevelSteps;
using UnityEngine;

namespace Game.Scripts
{
    [CreateAssetMenu(menuName = "Create LevelConfig", fileName = "Game/LevelConfig")]
    public class LevelConfig : ScriptableObject
    {
        public AnimationCurve DifficultyCurve;
        public float LevelDuration;
        public List<GuestProfile> AvailableGuests;
        
        public int BaseCreditWallet; 
        public float SpawnIntervalMin = 2f; 
        public float SpawnIntervalMax = 5f;
        
        [SerializeReference, SubclassSelector]
        public List<LevelStep> LevelStates = new();
        
        [SerializeReference, SubclassSelector]
        public EcsSystemsFactory EcsSystemFactory;

        public GameObject LevelLayout;
    }
}
