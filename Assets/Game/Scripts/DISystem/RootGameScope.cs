using Cysharp.Threading.Tasks;
using Game.Scripts.Infrastructure;
using Game.Scripts.Input;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using VContainer;
using VContainer.Unity;

namespace Game.Scripts.DISystem
{
    public class RootGameScope : LifetimeScope
    {
        [Header("Input")] 
        [SerializeField] private InputActionAsset actions;
        [Header("Sound")]
        [SerializeField] private SoundManager soundPrefab;
        protected override void Configure(IContainerBuilder builder)
        {
            Debug.Log("RootGameScope : Configure");
            builder.RegisterEntryPoint<GeneralBootstrap>(Lifetime.Singleton);
            builder.Register<SceneController>(Lifetime.Singleton);
            builder.Register<GameResources>(Lifetime.Singleton);
            builder.Register<SaveService>(Lifetime.Singleton);
            builder.Register<PlayerSessionService>(Lifetime.Singleton);
            
            builder.RegisterInstance(actions).AsSelf();
            
            builder.RegisterComponentInNewPrefab(soundPrefab, Lifetime.Singleton)
                .UnderTransform(this.transform)
                .AsSelf();
        }
    }

    public class GeneralBootstrap : IInitializable
    {
        private SceneController _sceneController;
        private SaveService _saveService;
        public GeneralBootstrap(SceneController sceneController, SaveService saveService)
        {
            _sceneController = sceneController;
            _saveService = saveService;
        }

        public void Initialize()
        {
            QualitySettings.vSyncCount = 1;
            if (_saveService.Data.IsFirstLaunch)
            {
                _saveService.Data.ContinueCompany = false;
                _saveService.Data.CurrentLevelIndex = 0;
                _saveService.Save();
                _sceneController.LoadMainGameSceneAsync().Forget();
            }
            else
            {
                _sceneController.LoadMainMenuSceneAsync().Forget();
            }
        }
    }
}