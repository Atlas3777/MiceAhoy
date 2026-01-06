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
                if (!_placementAspect.CreateGameObjectEventPool.Has(entityPlayer))
                    _placementAspect.CreateGameObjectEventPool.Add(entityPlayer);
                ref var createGO = ref _placementAspect.CreateGameObjectEventPool.Get(entityPlayer);
                createGO.objects ??= new();
                createGO.objects.Add((scrollMenuManager.SelectedFurniture,new Vector3Int()));
                createGO.destroyInvoker = false;
                scrollMenuManager.DeleteFurnitureFromCurrentList(scrollMenuManager.SelectedFurniture);
            }
        }
    }

    public void Destroy()
    {
        _iteratorPlayer = null;
    }
}
