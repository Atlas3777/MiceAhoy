using Game.Scripts;
using Leopotam.EcsProto;
using Leopotam.EcsProto.QoL;
using UnityEngine;

public class PlayerSpawnFurnitureSystem : IProtoInitSystem, IProtoRunSystem, IProtoDestroySystem
{
    [DI] readonly PlayerAspect _playerAspect;
    [DI] readonly PlacementAspect _placementAspect;
    private ScrollMenuManager scrollMenuManager;

    private ProtoIt _iteratorPlayer;
    private ProtoWorld _world;

    public PlayerSpawnFurnitureSystem(ScrollMenuManager scrollMenuManager)
    {
        this.scrollMenuManager = scrollMenuManager;
    }

    public void Init(IProtoSystems systems)
    {
        _world = systems.World();
        _iteratorPlayer = new(new[] { typeof(PlayerInputComponent), typeof(PlacementModeTag) });
        _iteratorPlayer.Init(_world);
    }

    public void Run()
    {
        foreach (var entityPlayer in _iteratorPlayer)
        {
            ref var playerInput = ref _playerAspect.InputRawPool.Get(entityPlayer);
            if (!playerInput.OpenScrollPressed || !playerInput.IsInPlacementMode) continue;

            Debug.Log("O ���� ������");

            if (playerInput.IsScrollMenuOpened)
            {
                playerInput.IsScrollMenuOpened = false;
                scrollMenuManager.HideScrollMenu();
                continue;
            }

            playerInput.IsScrollMenuOpened = true;
            scrollMenuManager.ShowScrollMenu();
        }
    }

    public void Destroy()
    {
        _iteratorPlayer = null;
    }
}
