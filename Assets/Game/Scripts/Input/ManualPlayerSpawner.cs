using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.Users;
using VContainer;
using VContainer.Unity;

namespace Game.Scripts.Input
{
    public class ManualPlayerSpawner : IStartable
    {
        private readonly IObjectResolver _objectResolver;
        private readonly GameObject _playerPrefab;
        private readonly Transform _playerSpawnPoint;
        private readonly CinemachineTargetGroup _cinemachineTargetGroup;

        public ManualPlayerSpawner(IObjectResolver resolver, GameResources gameResources, SpawnRegistry spawnRegistry, CinemachineTargetGroup  cinemachineTargetGroup)
        {
            _objectResolver = resolver;
            _playerPrefab = gameResources.Player.gameObject;
            _playerSpawnPoint = spawnRegistry.PlayerSpawn;
            _cinemachineTargetGroup =  cinemachineTargetGroup;
        }

        public void Start()
        {
            InputUser.listenForUnpairedDeviceActivity = 4;
            InputUser.onUnpairedDeviceUsed += Join;
        }

        private void Join(InputControl input, InputEventPtr eventPtr)
        {
            Debug.Log($"ManualPlayerSpawner: join {input.device} {eventPtr.type}");
            var player = _objectResolver.Instantiate(_playerPrefab, _playerSpawnPoint.position, _playerSpawnPoint.rotation );
            var target = new CinemachineTargetGroup.Target
            {
                Object = player.transform,
                Weight = 1,
                Radius = 1f,
            };
            _cinemachineTargetGroup.Targets.Add(target);
        }
    }
}