using Leopotam.EcsProto;
using Leopotam.EcsProto.QoL;
using UnityEngine;

namespace Game.Scripts.Systems
{
    public class PlayerInitializeInputSystem : IProtoInitSystem, IProtoRunSystem
    {
        [DI] private PlayerAspect _playerAspect;
        [DI] private ProtoWorld _world;
        private ProtoIt _playerInitializeIt;
        private readonly InputService _inputService;

        public PlayerInitializeInputSystem(InputService inputService)
            => _inputService = inputService;
    
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
                if (_inputService.TryGetPendingPlayerIndex(out var newIndex))
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
}