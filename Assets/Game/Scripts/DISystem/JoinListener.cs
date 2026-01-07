using Game.Scripts.Input;
using UnityEngine.InputSystem;

namespace Game.Scripts.DISystem
{
    public class JoinListener
    {
        private InputActionAsset _actions;

        private InputAction _join;
        private PlayerSpawner _spawner;

        public JoinListener(PlayerSpawner spawner, InputActionAsset actions)
        {
            _spawner = spawner;
            _actions = actions;
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
            _spawner.TrySpawn(ctx.control.device);
        }
    }
}