using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Game.Script.DISystem;
using PrimeTween;
using Unity.Cinemachine;
using UnityEngine;
using VContainer;

namespace Game.Scripts.LevelSteps
{
    [Serializable]
    public class GameplayStep : LevelStep
    {
        public override string Description => "Основная игра";

        public override GameplayPhase? Phase => GameplayPhase.Gameplay;
        public override async UniTask Execute(IObjectResolver resolver, CancellationToken ct)
        {
            
        }
    }
}