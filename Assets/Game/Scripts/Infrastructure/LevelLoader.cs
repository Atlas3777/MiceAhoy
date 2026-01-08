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

        public LevelLoader(LifetimeScope parentScope, SaveService service, LevelLifetimeScope levelScopePrefab)
        {
            _parentScope = parentScope;
            _saveService = service;
            _levelScopePrefab = levelScopePrefab;
        }

        public void Start()
        {
            Debug.Log("GameLifetimeScope : Start");
            var i = _saveService.Data.LevelIndex;
            var config = Resources.Load<LevelConfig>($"LevelConfigs/LevelConfig{i}");

            if (config == null) Debug.LogError($"Не нашел конфиг уровня {i}!");

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
