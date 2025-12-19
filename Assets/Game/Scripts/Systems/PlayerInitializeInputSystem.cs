using Leopotam.EcsProto;
using Leopotam.EcsProto.QoL;
using Leopotam.EcsProto.Unity;
using UnityEngine;

public class PlayerInitializeInputSystem : IProtoInitSystem, IProtoRunSystem
{
    [DIUnity("InputService")] readonly InputService _inputService = default;
    [DI] private PlayerAspect _playerAspect;
    [DI] private ProtoWorld _world;
    private ProtoIt _playerInitializeIt;

    public void Init(IProtoSystems systems)
    {
        _playerInitializeIt = new(new[]
            { typeof(PlayerInitializeEvent), typeof(PlayerInputComponent), typeof(PlayerIndexComponent) });
        _playerInitializeIt.Init(_world);
    }

    public void Run()
    {
        foreach (var playerInitEvent in _playerInitializeIt)
        {
            ref var playerIndexComp = ref _playerAspect.PlayerIndexPool.Get(playerInitEvent);
            
            // Пытаемся забрать индекс из очереди ожидающих
            if (_inputService.TryGetPendingPlayerIndex(out int newIndex))
            {
                playerIndexComp.PlayerIndex = newIndex;
                Debug.Log($"ECS Entity {playerInitEvent} assigned to Player Index {newIndex}");
            }
            else
            {
                Debug.LogError("Ошибка: Попытка инициализировать игрока в ECS, но в InputService нет свободных индексов в очереди!");
                // Можно назначить дефолтный 0 или обработать ошибку
                playerIndexComp.PlayerIndex = 0; 
            }
        }
    }
}