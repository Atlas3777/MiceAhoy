using System;
using Game.Scripts.Infrastructure;
using Leopotam.EcsProto;
using Leopotam.EcsProto.QoL;

namespace Game.Scripts.Systems
{
    public enum GameResult
    {
        None,
        Win,
        Lose
    }
    
    public class WinLoseSystem : IProtoRunSystem
    {
        public event Action<GameResult> OnGameFinished;
        private bool _isFinished = false;
        private readonly RuntimeLevelState _runtimeLevelState;

        public WinLoseSystem(RuntimeLevelState runtimeLevelState)
        {
            _runtimeLevelState = runtimeLevelState;
        }
        
        public void Run()
        {
            if (_isFinished) return;
            
            if (LoseCondition)
            {
                FinishGame(GameResult.Lose);
            }

            if (WinCondition)
            {
                FinishGame(GameResult.Win);
            }
        }
        
        private void FinishGame(GameResult result)
        {
            _isFinished = true;
            OnGameFinished?.Invoke(result);
        }

        private bool WinCondition
        {
            get
            {
                if (!_runtimeLevelState.TimedOut) return false;
                if (_runtimeLevelState.ActiveGuest == 0)
                    return true;

                return false;
            }
        }

        private bool LoseCondition
        {
            get
            {
                if (_runtimeLevelState.Reputation <= 0)
                    return true;
                return false;
            }
        }
    }
}