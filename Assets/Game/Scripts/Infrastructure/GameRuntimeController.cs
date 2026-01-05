using System;
using Game.Script.DISystem;
using Leopotam.EcsProto;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Game.Script.Infrastructure
{
    public class GameRuntimeController : IStartable, IFixedTickable, IDisposable
    {
        private readonly IProtoSystems _mainSystems;
        private readonly InputService _inputService;
        private readonly LoseGameSystem  _loseGameSystem;
        private readonly PauseView _pauseView;
        
        public bool IsPaused {get; private set;}

        public GameRuntimeController(
            [Key(IProtoSystemsType.MainSystem)] IProtoSystems mainSystems,
            InputService inputService,
            PauseView pauseView,
            LoseGameSystem loseGameSystem)
        {
            Debug.Log("Starting game state manager");
            _mainSystems = mainSystems;
            _inputService = inputService;
            _pauseView = pauseView;
            _loseGameSystem = loseGameSystem;
        }
        
        public void Start()
        {
            _mainSystems.Init();

            _inputService.OnPausePressed += OnPausePressed;
            
            IsPaused = false;
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
        }


        public void Dispose()
        {
            _mainSystems.Destroy();
            _inputService.OnPausePressed -= OnPausePressed;
            IsPaused = true;
        }

        public void FixedTick()
        {
            if(IsPaused) return;
            _mainSystems.Run();
        }
    }
}