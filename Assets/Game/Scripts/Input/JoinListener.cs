using Game.Scripts.Infrastructure;
using UnityEngine.InputSystem;

namespace Game.Scripts.Input
{
    public class JoinListener
    {
        private InputActionAsset _actions;
        private InputAction _join;
        private PlayerSpawner _spawner;
        private PlayerSessionService _sessionService;
        public JoinListener(PlayerSpawner spawner, InputActionAsset actions, PlayerSessionService sessionService)
        {
            _spawner = spawner;
            _actions = actions;
            _sessionService = sessionService;
            _join = _actions.FindActionMap("Join").FindAction("Join");
        }

        public void Enable()
        {
            _join.performed += OnJoin;
            _join.Enable();
        }

        public void Disable()
        {
            _join.performed -= OnJoin;
            _join.Disable();
        }

        private void OnJoin(InputAction.CallbackContext ctx)
        {
            var device = ctx.control.device;
            
            _sessionService.Join(device);
            _spawner.TrySpawn(device);
        }
    }
}