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

        public ManualPlayerSpawner(IObjectResolver resolver, GameResources gameResources)
        {
            _objectResolver = resolver;
            _gameResources =  gameResources;
        }
        
        public void Start()
        {
            InputUser.listenForUnpairedDeviceActivity = 4;
            InputUser.onUnpairedDeviceUsed += Join;
        }
        
        private void Join(InputControl input, InputEventPtr eventPtr)
        {
            Debug.Log($"ManualPlayerSpawner: join {input.device} {input.displayName}");
            var player = _gameResources.Player.gameObject;
            _objectResolver.Instantiate(player);
        }
    }
}