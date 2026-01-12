using Cysharp.Threading.Tasks;
using UnityEngine;
using VContainer.Unity;
using Game.Scripts.DISystem;
using VContainer;

namespace Game.Scripts.Infrastructure
{
    public class LevelLoader : IStartable
    {
        private readonly LifetimeScope _parentScope;
        private readonly LevelLifetimeScope _levelScopePrefab;
        private readonly SaveService _saveService;

        private LevelLifetimeScope _currentLevelScope;
        private SceneController _sc;

        public LevelLoader(LifetimeScope parentScope, SaveService service, LevelLifetimeScope levelScopePrefab, SceneController sc)
        {
            _parentScope = parentScope;
            _saveService = service;
            _levelScopePrefab = levelScopePrefab;
            _sc = sc;
        }

        public void Start()
        {
            Debug.Log("GameLifetimeScope : Start");
            _saveService.Data.IsFirstLaunch = false;
            _saveService.Save();

            int i;
            if (_saveService.Data.ContinueCompany)
                i = _saveService.Data.CompanyLevelIndex;
            else
                i = _saveService.Data.CurrentLevelIndex;

            var config = Resources.Load<LevelConfig>($"LevelConfigs/LevelConfig{i}");

            if (config == null)
            {
                Debug.Log($"Не нашел конфиг уровня {i}!");
                _sc.LoadMainGameSceneAsync().Forget();
                return;
            }

            LoadLevel(config);
        }

        public void LoadLevel(LevelConfig levelConfig)
        {
            UnloadLevel();

            var levelLayout = Object.Instantiate(levelConfig.LevelLayout);
            var context = levelLayout.GetComponent<LevelContext>();

            var levelScope = Object.Instantiate(_levelScopePrefab, _parentScope.transform);
    
            levelScope.SetConfig(levelConfig, context);
            levelScope.parentReference.Object = _parentScope;

            levelScope.Build(); 

            _currentLevelScope = levelScope;
        }

        public void UnloadLevel()
        {
            if (_currentLevelScope != null)
            {
                Object.Destroy(_currentLevelScope.gameObject);
                _currentLevelScope = null;
            }
        }
    }
}
