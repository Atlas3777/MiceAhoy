using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.Users;
using VContainer;
using VContainer.Unity;

namespace Game.Script.Input
{
    public class ManualPlayerSpawner : IStartable
    {
        private readonly IObjectResolver _objectResolver;
        private readonly GameResources _gameResources;
        private readonly CinemachineTargetGroup _cinemachineTargetGroup;

        public ManualPlayerSpawner(IObjectResolver resolver, GameResources gameResources, CinemachineTargetGroup targetGroup)
        {
            _objectResolver = resolver;
            _gameResources = gameResources;
            _cinemachineTargetGroup = targetGroup;
        }
        
        public void Start()
        {
            InputUser.listenForUnpairedDeviceActivity = 4;
            InputUser.onUnpairedDeviceUsed += Join;
        }
        
        private void Join(InputControl input, InputEventPtr eventPtr)
        {
            Debug.Log($"ManualPlayerSpawner: join {input.device} {eventPtr.type}");
            var playerPrefab = _gameResources.Player.gameObject;
            var player = _objectResolver.Instantiate(playerPrefab);
            var target = new CinemachineTargetGroup.Target
            {
                Object = player.transform,
                Weight = 1,
                Radius = 0.5f,
            };
            _cinemachineTargetGroup.Targets.Add(target);
        }
    }
}