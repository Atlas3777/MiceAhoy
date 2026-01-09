using Leopotam.EcsProto.QoL;
using Leopotam.EcsProto;
using UnityEngine;
using UnityEngine.LightTransport;

public class PlayerPressedPSystem : IProtoRunSystem, IProtoInitSystem, IProtoDestroySystem
{
    [DI] readonly PlayerAspect _playerAspect;
    [DI] readonly PlacementAspect _placementAspect;

    private ProtoIt _iterator;
    private ProtoWorld _world;
    private ScrollMenuManager scrollMenuManager;

    private bool isPlacementStarted = false;
    private bool isPlacementFinished = false;

    public PlayerPressedPSystem(ScrollMenuManager scrollMenuManager) =>
        this.scrollMenuManager = scrollMenuManager;

    public void StartPlacementMode() => isPlacementStarted = true;

    public void EndPlacementMode() => isPlacementFinished = true;

    public void Init(IProtoSystems systems)
    {
        _world = systems.World();
        _iterator = new(new[] { typeof(PlayerInputComponent) });
        _iterator.Init(_world);
    }

    public void Run()
    {
        //delete this --> 
        if (!isPlacementFinished && !isPlacementStarted)
        {
            foreach (var entityPlayer in _iterator)
            {
                ref var playerInput = ref _playerAspect.InputRawPool.Get(entityPlayer);
                if (playerInput.start) { StartPlacementMode(); playerInput.start = false; }
                if (playerInput.end) { EndPlacementMode(); playerInput.end = false; }

            }
        }
        // <--

        if (isPlacementStarted)
        {
            foreach (var entityPlayer in _iterator)
            {
                ref var playerInput = ref _playerAspect.InputRawPool.Get(entityPlayer);
                if (playerInput.IsInPlacementMode) continue;
                playerInput.IsInPlacementMode = true;
                if (!_placementAspect.PlacementModeTagPool.Has(entityPlayer))
                    _placementAspect.PlacementModeTagPool.Add(entityPlayer);
            }
            isPlacementStarted = false;
        }
        if (isPlacementFinished)
        {
            foreach (var entityPlayer in _iterator)
            {
                ref var playerInput = ref _playerAspect.InputRawPool.Get(entityPlayer);
                if (!playerInput.IsInPlacementMode) continue;
                playerInput.IsInPlacementMode = false;
                playerInput.IsScrollMenuOpened = false;
                scrollMenuManager.ClearScrollMenu();
                scrollMenuManager.HideScrollMenu();
                if (!_placementAspect.ActivateAllSpawnersEventPool.Has(entityPlayer))
                    _placementAspect.ActivateAllSpawnersEventPool.Add(entityPlayer);
            }
            isPlacementFinished = false;
        }
    }

    public void Destroy()
    {
        _iterator = null;
    }
}