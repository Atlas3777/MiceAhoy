using Leopotam.EcsProto;
using Leopotam.EcsProto.QoL;
using Leopotam.EcsProto.Unity;
using UnityEngine;

class UpdateInputSystem : IProtoInitSystem, IProtoRunSystem, IProtoDestroySystem    
{
    [DIUnity ("InputService")] readonly InputService _inputService = default;
    [DI] readonly ProtoWorld _world = default;
    
    private PlayerAspect _playerAspect;
    private ProtoIt _iterator;

    public void Init(IProtoSystems systems)
    {
        _playerAspect = (PlayerAspect)_world.Aspect(typeof(PlayerAspect));
        
        _iterator = new(new[] { typeof(PlayerInputComponent), typeof(PlayerIndexComponent) });
        _iterator.Init(_world);
    }

    public void Run()
    {
        foreach (var entity in _iterator)
        {
            ref var playerInputComponent = ref _playerAspect.InputRawPool.Get(entity);
            ref var index = ref _playerAspect.PlayerIndexPool.Get(entity).PlayerIndex;
            
            ref var playerIndex = ref _playerAspect.PlayerIndexPool.Get(entity);
            
            var playerInputData = _inputService.GetPlayerInputState(playerIndex.PlayerIndex);
            if (index == 1)
            {
                Debug.Log(playerInputData.MoveDirection);
            }
            
                
            playerInputComponent.MoveDirection = playerInputData.MoveDirection;
            playerInputComponent.InteractPressed = playerInputData.InteractPressed;
            playerInputComponent.RandomSpawnFurniturePressed = playerInputData.RandomSpawnFurniturePressed;
            playerInputComponent.MoveFurniturePressed = playerInputData.MoveFurniturePressed;
            playerInputComponent.PickPlacePressed = playerInputData.PickPlacePressed;
        }
    }

    public void Destroy()
    {
        _playerAspect = null;
        _iterator = null;
    }
}