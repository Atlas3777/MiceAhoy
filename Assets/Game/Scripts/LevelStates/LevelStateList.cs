using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using VContainer;

namespace Game.Scripts.LevelStates
{
    [CreateAssetMenu(fileName = "LevelStateList", menuName = "Game/LevelStateList")]
    public class LevelStateList : ScriptableObject
    {
        [SerializeReference, SubclassSelector]
        public List<LevelState> LevelStates = new();
    }
    
    [Serializable]
    public abstract class LevelState
    {
        public abstract string Description { get; }

        public virtual GameplayPhase? Phase => null;

        public abstract UniTask Execute(
            IObjectResolver resolver,
            CancellationToken ct);

        public virtual void Exit() {}
    }
}