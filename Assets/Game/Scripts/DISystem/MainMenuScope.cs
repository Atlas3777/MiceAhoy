using Game.Scripts.Infrastructure;
using Game.Scripts.Input;
using UnityEngine;
using UnityEngine.InputSystem;
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
            builder.Register<MainMenuJoinHandler>(Lifetime.Singleton).AsImplementedInterfaces().AsSelf();
        }
    }
    public class MainMenuJoinHandler : IInitializable
    {
        private readonly PlayerSessionService _session;
        private readonly InputActionAsset _actions;

        public MainMenuJoinHandler(PlayerSessionService session, InputActionAsset actions)
        {
            _session = session;
            _actions = actions;
        }

        public void Initialize()
        {
            _actions.FindActionMap("UI").actionTriggered += context => 
            {
                if (context.performed) 
                    _session.Join(context.control.device);
            };
        }
    }
}