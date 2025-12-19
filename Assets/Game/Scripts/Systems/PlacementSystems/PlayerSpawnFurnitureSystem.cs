using Leopotam.EcsProto;
using Leopotam.EcsProto.QoL;
using UnityEngine;

public class PlayerSpawnFurnitureSystem : IProtoInitSystem, IProtoRunSystem, IProtoDestroySystem
{
    [DI] readonly PlayerAspect _playerAspect;
    [DI] readonly PlacementAspect _placementAspect;

    private PlacementGrid worldGrid;
    private ProtoIt _iterator;
    private ProtoWorld _world;

    public PlayerSpawnFurnitureSystem(PlacementGrid placementGrid) =>
        worldGrid = placementGrid;

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
            if (!playerInput.RandomSpawnFurniturePressed) continue;

            Debug.Log("P была нажата");

            if (playerInput.IsInPlacementMode)
            {
                playerInput.IsInPlacementMode = false;
                if (!_placementAspect.DestroyAllSpawnersEventPool.Has(entityPlayer))
                    _placementAspect.DestroyAllSpawnersEventPool.Add(entityPlayer);
                continue;
            }

            playerInput.IsInPlacementMode = true;
            var emptyPlace = GetFirstEmptyCell();
            var currentType = typeof(FridgeSpawner); //временно спавню только холодильники

            //добавляю event на игрока
            if (!_placementAspect.CreateSpawnersEventPool.Has(entityPlayer))
                _placementAspect.CreateSpawnersEventPool.Add(entityPlayer);
        }
    }

    private Vector2Int GetFirstEmptyCell()
    {
        for (int x = 0; x < worldGrid.PlacementZoneSize.x; x++)
            for (int y = 0; y < worldGrid.PlacementZoneSize.y; y++)
            {
                var v = new Vector2Int(x, y);
                if (!worldGrid.IsContains(v))
                    return v;
            }
        return default;
    }

    public void Destroy()
    {
        _iterator = null;
    }
}
