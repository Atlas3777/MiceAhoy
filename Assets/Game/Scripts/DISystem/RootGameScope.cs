using Game.Scripts.Infrastructure;
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
            builder.Register<SceneController>(Lifetime.Singleton);
            builder.Register<GameResources>(Lifetime.Singleton);
            builder.Register<SaveService>(Lifetime.Singleton);
            
            builder.RegisterInstance(actions).AsSelf();
            
            builder.RegisterComponentInNewPrefab(soundPrefab, Lifetime.Singleton)
                .UnderTransform(this.transform)
                .AsSelf();
        }
    }

    public class SaveService
    {
        public int LevelIndex { get; set; } = 1;
    }
}