using System.Collections.Generic;
using System.Linq;
using Game.Scripts.Infrastructure;
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
        private readonly PlayerSessionService _sessionService;
        private readonly HashSet<InputDevice> _spawnedDevices = new();

        private Transform _spawnPoint;

        public PlayerSpawner(IObjectResolver resolver, GameResources resources, PlayerSessionService sessionService)
        {
            _resolver = resolver;
            _prefab = resources.Player.gameObject;
            _sessionService = sessionService;
        }

        public void SetSpawnPoint(Transform point)
        {
            _spawnPoint = point;
        }

        public void SpawnExistingPlayers()
        {
            if (!_sessionService.ActiveDevices.Any())
            {
                if (Keyboard.current != null)
                {
                    _sessionService.Join(Keyboard.current);
                }
            }

            foreach (var device in _sessionService.ActiveDevices)
            {
                if (device is Mouse) continue; 

                InternalSpawn(device);
            }
        }

        public void TrySpawn(InputDevice device)
        {
            if (_spawnedDevices.Contains(device))
                return;

            _sessionService.Join(device);
            
            InternalSpawn(device);
        }

        private void InternalSpawn(InputDevice device)
        {
            if (device == null) return;

            var player = _resolver.Instantiate(
                _prefab,
                _spawnPoint.position,
                _spawnPoint.rotation
            );

            var playerInput = player.GetComponent<PlayerInput>();
            
            InputUser.PerformPairingWithDevice(device, playerInput.user);
            
            _spawnedDevices.Add(device);
            
            Debug.Log($"[PlayerSpawner] Player spawned for device: {device.name}");
        }
    }
}