using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Game.Scripts.DISystem
{
    public class MainMenuScope : LifetimeScope
    {
        [SerializeField] private MainMenuUIController mainMenuUIController;
        protected override void Configure(IContainerBuilder builder)
        {
            Debug.Log("MainMenuScope : Configure");

            builder.RegisterEntryPoint<MainMenuBootstrap>();
            builder.RegisterInstance(mainMenuUIController);
        }
    }
}