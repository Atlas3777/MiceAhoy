using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Game.Script.DISystem
{
    public class MainMenuScope : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            Debug.Log("MainMenuScope : LifetimeScope");
        }
    }
}