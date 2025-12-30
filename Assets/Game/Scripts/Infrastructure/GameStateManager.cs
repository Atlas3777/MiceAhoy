using System;
using Game.Script.DISystem;
using Leopotam.EcsProto;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Game.Script.Infrastructure
{
    public enum GameState
    {
        Lose,
        Win
    }
    
    public class GameStateManager : IStartable, ITickable, IDisposable
    {
        private IProtoSystems _mainSystems;
        //private IProtoSystems _physicsSystems;
        private InputService _inputService;
        
        private EndGameSystem  _endGameSystem;
        private UIController _uiController;
        
        private bool IsPaused = true;
        
        public Action<GameState> EndGame;

        public GameStateManager(
            [Key(IProtoSystemsType.MainSystem)] IProtoSystems mainSystems,
            //[Key(IProtoSystemsType.PhysicsSystem)] IProtoSystems physicsSystems,
            InputService inputService,
            UIController uiController,
            EndGameSystem endGameSystem)
        {
            Debug.Log("Starting game state manager");
            _mainSystems = mainSystems;
            //_physicsSystems = physicsSystems;
            _inputService = inputService;
            _uiController = uiController;
            _endGameSystem = endGameSystem;
        }
        
        public void Start()
        {
            _mainSystems.Init();
            //_physicsSystems.Init();

            _endGameSystem.EndGame += EndGameHandler;
            _inputService.OnPausePressed += OnPausePressed;
            
            IsPaused = false;
        }

        private void EndGameHandler(GameState gameState)
        {
            if (gameState == GameState.Lose)
                _uiController.ShowLose();
            else if (gameState == GameState.Win)
                _uiController.ShowWin();
        }

        private void OnPausePressed()
        {
            if (!IsPaused)
            {
                IsPaused = true;
                _inputService.SwitchAllActionMapsTo("UI");
                _uiController.OpenPauseMenu();
                Time.timeScale = 0;
                Debug.Log("Game Paused. Input Map switched to UI.");
            }
            else
            {
                IsPaused = false;
                _inputService.SwitchAllActionMapsTo("Player");
                _uiController.ClosePauseMenu();
                Time.timeScale = 1;
                Debug.Log("Game Unpaused. Input Map switched to Player.");
            }
        }

        public void Tick()
        {
            if(IsPaused) return;
            _mainSystems.Run();
        }

        // public void FixedTick()
        // {
        //     if (IsPaused) return;
        //     _physicsSystems.Run();
        // }

        public void Dispose()
        {
            _mainSystems.Destroy();
            //_physicsSystems.Destroy();
            _inputService.OnPausePressed -= OnPausePressed;
            IsPaused = true;
        }
    }
}