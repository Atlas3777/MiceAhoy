using Game.Scripts.Infrastructure;
using Game.Scripts.UIControllers;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Game.Scripts.DISystem
{
    public class MainMenuScope : LifetimeScope
    {
        [SerializeField] private MainMenuUIController mainMenuUIController;
        [SerializeField] private SelectLevelUI selectLevelUI;
        protected override void Configure(IContainerBuilder builder)
        {
            Debug.Log("MainMenuScope : Configure");

            builder.RegisterEntryPoint<MainMenuBootstrap>();
            builder.Register<SelectLevelUIController>(Lifetime.Singleton);
            
            builder.RegisterInstance(mainMenuUIController).AsImplementedInterfaces().AsSelf();
            builder.RegisterInstance(selectLevelUI);
            builder.Register<MainMenuJoinHandler>(Lifetime.Singleton).AsImplementedInterfaces().AsSelf();
        }
    }
}