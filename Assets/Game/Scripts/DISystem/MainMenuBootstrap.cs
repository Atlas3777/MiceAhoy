using Cysharp.Threading.Tasks;
using Game.Scripts.Infrastructure;
using UnityEngine;
using VContainer.Unity;

namespace Game.Scripts.DISystem
{
    public class MainMenuBootstrap : IStartable
    {
        private readonly SoundManager _soundManager;
        private readonly GameResources _gameResources;
        private readonly MainMenuUIController _ui;
        private readonly SceneController _sceneController;

        public MainMenuBootstrap(
            SoundManager soundManager, 
            GameResources gameResources, 
            MainMenuUIController ui,
            SceneController sceneController)
        {
            _soundManager = soundManager;
            _gameResources = gameResources;
            _ui = ui;
            _sceneController = sceneController;
        }

        public void Start()
        {
            SubscribeEvents();
            PlayMenuMusic().Forget();
        }
        private async UniTaskVoid PlayMenuMusic()
        {
            var music = _gameResources.SoundsLink.fonovyy_zvuk_krik_chaek_shum_vody;
            await _soundManager.PlayMusicAsync(music);
        }
        
        private void SubscribeEvents()
        {
            _ui.startButton.onClick.AddListener(() => _sceneController.LoadMainGameSceneAsync().Forget());
            _ui.settingsButton.onClick.AddListener(() => Debug.Log("settingsButton.onClick"));
            _ui.tutorialButton.onClick.AddListener(() => _sceneController.LoadTutorialSceneAsync().Forget());
            _ui.shopButton.onClick.AddListener(() => _sceneController.LoadShopSceneAsync().Forget());
            _ui.exitButton.onClick.AddListener(() => _sceneController.ExitApplication());
        }
    }
}