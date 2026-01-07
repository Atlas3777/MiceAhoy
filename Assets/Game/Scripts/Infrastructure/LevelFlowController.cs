using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using Game.Scripts.LevelSteps;
using UnityEngine;
using VContainer;

namespace Game.Scripts.Infrastructure
{
    public interface ILevelStateContext
    {
        public GameplayPhase GetCurrentLevelPhase();
    }
    
    public enum GameplayPhase
    {
        None,
        Build,
        Gameplay,
        EcsPause
    }
    
    public sealed class LevelFlowController :  ILevelStateContext, IDisposable
    {
        private readonly IObjectResolver _resolver;
        private List<LevelStep> _steps;

        private CancellationTokenSource _cts;
        private GameplayPhase _currentState = GameplayPhase.None;
        private int _index = -1;
        private bool _running = false;

        public LevelFlowController(IObjectResolver resolver)
        {
            _resolver = resolver ?? throw new ArgumentNullException(nameof(resolver));
        }

        public void Start(List<LevelStep> steps)
        {
            if (_running) return;
            _steps = steps ?? throw new ArgumentNullException(nameof(steps));
            
            _running = true;
            _cts = new CancellationTokenSource();
            _index = -1;
            ProceedToNext();
        }

        public void Stop()
        {
            if (!_running) return;
            _cts?.Cancel();
            _running = false;
        }

        public void ProceedToNext()
        {
            if (_cts == null || _cts.IsCancellationRequested) return;
            Debug.Log("ProceedToNext");

            _index++;

            if (_index >= _steps.Count)
            {
                _running = false;
                return;
            }

            RunState(_steps[_index], _cts.Token).Forget();
        }

        async UniTaskVoid RunState(LevelStep step, CancellationToken ct)
        {
            if (step == null) return;

            try
            {
                if (step.Phase.HasValue)
                {
                    _currentState = step.Phase.Value;
                }


                Debug.Log(step.Description);
                await step.Execute(_resolver, ct).AttachExternalCancellation(ct);
            }
            catch (OperationCanceledException)
            {
                Debug.Log("[LevelStateMachine] RunState cancelled.");
                return;
            }
            catch (Exception ex)
            {
                Debug.LogException(ex);
            }
            finally
            {
                try
                {
                    step.Exit();
                }
                catch (Exception ex)
                {
                    Debug.LogException(ex);
                }
            }

            ProceedToNext();
        }

        public GameplayPhase GetCurrentLevelPhase() => _currentState;
        public void SetCurrentLevelPhase(GameplayPhase phase) => _currentState = phase;

        public void Dispose()
        {
            _cts?.Cancel();
            _cts?.Dispose();
            _cts = null;
            _running = false;
        }
    }
}
