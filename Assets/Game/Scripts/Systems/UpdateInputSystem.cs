using Leopotam.EcsProto;
using Leopotam.EcsProto.QoL;
using UnityEngine;

class UpdateInputSystem : IProtoInitSystem, IProtoRunSystem 
{
    [DI] readonly ProtoWorld _world;
    [DI] readonly PlayerAspect _playerAspect; 
    
    private ProtoIt _iterator;
    private readonly InputService _inputService;

    private const float LookDeadZone = 0.1f;
    
    public UpdateInputSystem(InputService inputService) {
        _inputService = inputService;
    }

    public void Init(IProtoSystems systems) {
        _iterator = new ProtoIt(new[] { typeof(PlayerInputComponent), typeof(PlayerIndexComponent) });
        _iterator.Init(_world);
    }

    public void Run() {
        foreach (var entity in _iterator) {
            ref var input = ref _playerAspect.InputRawPool.Get(entity);
            ref var playerIndex = ref _playerAspect.PlayerIndexPool.Get(entity);
            
            var data = _inputService.GetPlayerInputState(playerIndex.PlayerIndex);
          
            input.MoveDirection = data.MoveDirection;

            if (input.MoveDirection.sqrMagnitude > LookDeadZone * LookDeadZone) {
                input.LookDirection = input.MoveDirection.normalized;
            }
            
            input.InteractPressed = data.InteractPressed;
            input.PickPlacePressed = data.PickPlacePressed;
        }
    }
}