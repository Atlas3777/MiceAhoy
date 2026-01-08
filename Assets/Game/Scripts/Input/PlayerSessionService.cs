using System.Collections.Generic;
using UnityEngine.InputSystem;

namespace Game.Scripts.Input
{
    public class PlayerSessionService
    {
        private readonly HashSet<InputDevice> _activeDevices = new();
        public IEnumerable<InputDevice> ActiveDevices => _activeDevices;

        public PlayerSessionService()
        {
            InputSystem.onDeviceChange += OnDeviceChange;
        }

        public void Join(InputDevice device)
        {
            if (device == null) return;
            _activeDevices.Add(device);
        }

        public void Leave(InputDevice device)
        {
            _activeDevices.Remove(device);
        }

        private void OnDeviceChange(InputDevice device, InputDeviceChange change)
        {
            if (change == InputDeviceChange.Removed)
            {
                _activeDevices.Remove(device);
            }
        }
    }
}