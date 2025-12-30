using Game.Script.Infrastructure;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Game.Scripts.DISystem
{
    public class RootGameScope : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            Debug.Log("RootGameScope : Configure");
            builder.Register<SceneController>(Lifetime.Singleton);
        }
    }
}