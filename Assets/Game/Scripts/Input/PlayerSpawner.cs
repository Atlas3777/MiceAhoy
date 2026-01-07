using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;
using VContainer;
using VContainer.Unity;

namespace Game.Scripts.Input
{
    public class PlayerSpawner
    {
        private readonly GameObject _prefab;
        private readonly IObjectResolver _resolver;
        private readonly HashSet<InputDevice> _usedDevices = new();

        private Transform _spawnPoint;

        public PlayerSpawner(IObjectResolver resolver, GameResources resources)
        {
            _resolver = resolver;
            _prefab = resources.Player.gameObject;
        }

        public void SetSpawnPoint(Transform point)
        {
            _spawnPoint = point;
        }

        public void TrySpawn(InputDevice device)
        {
            if (_usedDevices.Contains(device))
                return;

            var player = _resolver.Instantiate(
                _prefab,
                _spawnPoint.position,
                _spawnPoint.rotation
            );

            var playerInput = player.GetComponent<PlayerInput>();

            InputUser.PerformPairingWithDevice(device, playerInput.user);
            _usedDevices.Add(device);
        }
    }

}
