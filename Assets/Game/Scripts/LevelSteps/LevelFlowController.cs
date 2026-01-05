using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Game.Scripts.LevelSteps
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
    
    public sealed class LevelFlowController : IStartable, ILevelStateContext, IDisposable
    {
        private readonly List<LevelStep> _steps;
        private readonly IObjectResolver _resolver;

        private CancellationTokenSource _cts;
        private GameplayPhase _currentState = GameplayPhase.None;
        private int _index = -1;
        private bool _running = false;

        public event Action<string> StateStarted;
        public event Action StateFinished;
        public event Action AllStatesCompleted;

        public LevelFlowController(IObjectResolver resolver, LevelStepsList stepses)
        {
            _resolver = resolver ?? throw new ArgumentNullException(nameof(resolver));
            _steps = stepses.LevelStates ?? throw new ArgumentNullException(nameof(stepses));
        }

        public void Start()
        {
            if (_running) return;
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
                AllStatesCompleted?.Invoke();
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

                StateStarted?.Invoke(step.Description);

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

            StateFinished?.Invoke();

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
