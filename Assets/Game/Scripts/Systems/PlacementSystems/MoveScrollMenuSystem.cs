using Game.Scripts;
using Leopotam.EcsProto.QoL;
using Leopotam.EcsProto;
using UnityEngine;

public class MoveScrollMenuSystem : IProtoInitSystem, IProtoRunSystem, IProtoDestroySystem
{
    [DI] readonly PlayerAspect _playerAspect;
    [DI] readonly PlacementAspect _placementAspect;
    private ScrollMenuManager scrollMenuManager;

    private ProtoIt _iteratorPlayer;
    private ProtoWorld _world;

    public MoveScrollMenuSystem(ScrollMenuManager scrollMenuManager)
    {
        this.scrollMenuManager = scrollMenuManager;
    }

    public void Init(IProtoSystems systems)
    {
        _world = systems.World();
        _iteratorPlayer = new(new[] { typeof(PlayerInputComponent) });
        _iteratorPlayer.Init(_world);
    }

    public void Run()
    {
        foreach (var entityPlayer in _iteratorPlayer)
        {
            ref var input = ref _playerAspect.InputRawPool.Get(entityPlayer);
            if (!input.IsScrollMenuOpened) continue;

            if (input.IsLeftPressed) scrollMenuManager.MoveCursorLeft();
            if (input.IsRightPressed) scrollMenuManager.MoveCursorRight();

            if (input.InteractPressed)
            {
                if (!_placementAspect.CreateSpawnersEventPool.Has(entityPlayer))
                    _placementAspect.CreateSpawnersEventPool.Add(entityPlayer);
                ref var spawnerEvent = ref _placementAspect.CreateSpawnersEventPool.Get(entityPlayer);
                spawnerEvent.spawnerType = scrollMenuManager.SelectedFurniture;

                scrollMenuManager.DeleteFurnitureFromCurrentList(scrollMenuManager.SelectedFurniture);
            }
        }
    }

    public void Destroy()
    {
        _iteratorPlayer = null;
    }
}