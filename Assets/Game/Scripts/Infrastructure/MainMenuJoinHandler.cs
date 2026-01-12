using Game.Scripts.Input;
using UnityEngine.InputSystem;
using VContainer.Unity;

namespace Game.Scripts.Infrastructure
{
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