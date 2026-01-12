using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Game.Scripts.Infrastructure;
using Game.Scripts.UIControllers;
using VContainer.Unity;

namespace Game.Scripts.DISystem
{
    public class MainMenuBootstrap : IStartable, IDisposable
    {
        private readonly SoundManager _soundManager;
        private readonly GameResources _gameResources;
        private readonly MainMenuUIController _ui;
        private readonly SceneController _sceneController;
        private readonly SelectLevelUIController _selectLevelUIController;
        private readonly SaveService _saveService;

        public MainMenuBootstrap(
            SoundManager soundManager,
            GameResources gameResources,
            MainMenuUIController ui,
            SceneController sceneController,
            SelectLevelUIController selectLevelUControllerI, SaveService saveService)
        {
            _soundManager = soundManager;
            _gameResources = gameResources;
            _ui = ui;
            _sceneController = sceneController;
            _selectLevelUIController = selectLevelUControllerI;
            _saveService = saveService;
        }

        public void Start()
        {
            SubscribeEvents();
            _selectLevelUIController.Subscribe();
            PlayMenuMusic();
        }


        private void PlayMenuMusic()
        {
            var music = _gameResources.SoundsLink.menu_game;
            _soundManager.PlayMusicAsync(music, false).Forget();
        }

        private void SubscribeEvents()
        {
            _ui.startButton.onClick.AddListener(Continue);
            //_ui.settingsButton.onClick.AddListener(() => Debug.Log("settingsButton.onClick"));
            _ui.tutorialButton.onClick.AddListener(LoadTutorial);
            //_ui.shopButton.onClick.AddListener(() => _sceneController.LoadShopSceneAsync().Forget());
            _ui.exitButton.onClick.AddListener(() => _sceneController.ExitApplication());
        }

        private void Continue()
        {
            _saveService.Data.ContinueCompany = true;
            _saveService.Data.CurrentLevelIndex = _saveService.Data.CompanyLevelIndex;
            _saveService.Save();
            
            _sceneController.LoadMainGameSceneAsync().Forget();
        }
        
        private void LoadTutorial()
        {
            _saveService.Data.ContinueCompany = false;
            _saveService.Data.CurrentLevelIndex = 0;
            _saveService.Save();
            
            _sceneController.LoadMainGameSceneAsync().Forget();
        }

        public void Dispose()
        {
            
        }
    }
}