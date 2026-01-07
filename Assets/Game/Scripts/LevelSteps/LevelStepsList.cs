using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using Game.Scripts.Infrastructure;
using UnityEngine;
using VContainer;

namespace Game.Scripts.LevelSteps
{
    [CreateAssetMenu(fileName = "LevelStepsList", menuName = "Game/LevelStepsList")]
    public class LevelStepsList : ScriptableObject
    {
        [SerializeReference, SubclassSelector]
        public List<LevelStep> LevelStates = new();
    }
    
    [Serializable]
    public abstract class LevelStep
    {
        public abstract string Description { get; }

        public virtual GameplayPhase? Phase => null;

        public abstract UniTask Execute(
            IObjectResolver resolver,
            CancellationToken ct);

        public virtual void Exit() {}
    }
}