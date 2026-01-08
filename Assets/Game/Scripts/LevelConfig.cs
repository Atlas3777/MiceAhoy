using System.Collections.Generic;
using Game.Scripts.Infrastructure;
using Game.Scripts.LevelSettings;
using Game.Scripts.LevelSteps;
using UnityEngine;

namespace Game.Scripts
{
    [CreateAssetMenu(menuName = "Create LevelConfig", fileName = "Game/LevelConfig")]
    public class LevelConfig : ScriptableObject
    {
        [Header("Difficulty & intensity")]
        public AnimationCurve DifficultyCurve; // возвращает множитель (0..N) для BaseCreditsPerSecond
        public float BaseCreditsPerSecond = 1f; // значение, которое умножается на кривую -> creditsPerSecond
        public AnimationCurve IntensityCurve; // 0..1, влияет на паузы между спавнами и на вероятность дорогих гостей

        [Header("Level")]
        public float LevelDuration;
        public List<GuestProfile> AvailableGuests;

        [Header("Economy / spawn budget")]
        public int BaseCreditWallet;
        [Tooltip("Процент от максим. стоимости гостя, который всегда оставляем в резерве (0..1)")]
        [Range(0f, 0.9f)]
        public float MinBudgetReserveFraction = 0.15f;

        [Header("Spawn timing")]
        public float SpawnIntervalMin = 2f;
        public float SpawnIntervalMax = 5f;
        public float MinSpawnRetryInterval = 0.5f; // если стратегия вернула null, повторим через это время

        [Header("Concurrency")]
        public int MaxConcurrentGuests = 6;

        [Header("Director Logic")]
        [SerializeReference, SubclassSelector]
        public ISpawnStrategy SpawnStrategy;

        [SerializeReference, SubclassSelector]
        public List<LevelStep> LevelStates = new();

        [SerializeReference, SubclassSelector]
        public EcsSystemsFactory EcsSystemFactory;

        [Header("Misc")]
        public float ForcedSpawnAfterNoSpawnSeconds = 10f; // форсить самый дешёвый гостя если ничего не спавнилось
        public GameObject LevelLayout;
    }
}