using Game.Scripts.Infrastructure;
using UnityEngine;
using UnityEngine.InputSystem;
using VContainer;
using VContainer.Unity;

namespace Game.Scripts.DISystem
{
    public class RootGameScope : LifetimeScope
    {
        [Header("Input")] 
        [SerializeField] private InputActionAsset actions;
        protected override void Configure(IContainerBuilder builder)
        {
            Debug.Log("RootGameScope : Configure");
            builder.Register<SceneController>(Lifetime.Singleton);
            builder.Register<SaveService>(Lifetime.Singleton);
            builder.RegisterInstance(actions).AsSelf();
        }
    }

    public class SaveService
    {
        public int LevelIndex { get; set; } = 1;
    }
}