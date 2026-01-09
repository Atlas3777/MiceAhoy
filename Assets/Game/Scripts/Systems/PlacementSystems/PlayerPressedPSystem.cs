using Leopotam.EcsProto.QoL;
using Leopotam.EcsProto;
using UnityEngine;

public class PlayerPressedPSystem : IProtoInitSystem, IProtoRunSystem, IProtoDestroySystem
{
    [DI] readonly PlayerAspect _playerAspect;
    [DI] readonly PlacementAspect _placementAspect;

    private ProtoIt _iterator;
    private ProtoWorld _world;

    public void Init(IProtoSystems systems)
    {
        _world = systems.World();
        _iterator = new(new[] { typeof(PlayerInputComponent) });
        _iterator.Init(_world);
    }

    public void Run()
    {
        foreach (var entityPlayer in _iterator)
        {
            ref var playerInput = ref _playerAspect.InputRawPool.Get(entityPlayer);
            if (!playerInput.PlacementModePressed) continue;

            Debug.Log("P была нажата");

            if (playerInput.IsInPlacementMode)
            {
                playerInput.IsInPlacementMode = false;
                if (!_placementAspect.ActivateAllSpawnersEventPool.Has(entityPlayer))
                    _placementAspect.ActivateAllSpawnersEventPool.Add(entityPlayer);
                continue;
            }

            playerInput.IsInPlacementMode = true;

            //добавл€ю tag на игрока
            if (!_placementAspect.PlacementModeTagPool.Has(entityPlayer))
                _placementAspect.PlacementModeTagPool.Add(entityPlayer);
        }
    }

    public void Destroy()
    {
        _iterator = null;
    }
}