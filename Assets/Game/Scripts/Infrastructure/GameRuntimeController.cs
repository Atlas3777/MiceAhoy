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
    
    public class GameRuntimeController : IStartable, ITickable, IDisposable
    {
        private readonly IProtoSystems _mainSystems;
        private readonly InputService _inputService;
        private readonly EndGameSystem  _endGameSystem;
        private readonly PauseView _pauseView;
        
        public bool IsPaused {get; private set;}
        public Action<GameState> EndGame;

        public GameRuntimeController(
            [Key(IProtoSystemsType.MainSystem)] IProtoSystems mainSystems,
            InputService inputService,
            PauseView pauseView,
            EndGameSystem endGameSystem)
        {
            Debug.Log("Starting game state manager");
            _mainSystems = mainSystems;
            _inputService = inputService;
            _pauseView = pauseView;
            _endGameSystem = endGameSystem;
        }
        
        public void Start()
        {
            _mainSystems.Init();

            _endGameSystem.EndGame += EndGameHandler;
            _inputService.OnPausePressed += OnPausePressed;
            
            IsPaused = false;
        }

        private void EndGameHandler(GameState gameState)
        {
            if (gameState == GameState.Lose)
            {
                // _uiController.ShowLose();
                Debug.Log("Game state lost");
            }
            else if (gameState == GameState.Win)
            {
                // _uiController.ShowWin();
                Debug.Log("Game state win");
                
            }
        }

        private void OnPausePressed()
        {
            if (!IsPaused)
            {
                IsPaused = true;
                _inputService.SwitchAllActionMapsTo("UI");
                _pauseView.OpenPauseMenu();
                Time.timeScale = 0;
                Debug.Log("Game Paused. Input Map switched to UI.");
            }
            else
            {
                IsPaused = false;
                _inputService.SwitchAllActionMapsTo("Player");
                _pauseView.ClosePauseMenu();
                Time.timeScale = 1;
                Debug.Log("Game Unpaused. Input Map switched to Player.");
            }
        }

        public void Tick()
        {
            if(IsPaused) return;
            _mainSystems.Run();
        }


        public void Dispose()
        {
            _mainSystems.Destroy();
            _inputService.OnPausePressed -= OnPausePressed;
            IsPaused = true;
        }
    }
}