using Leopotam.EcsProto;
using Leopotam.EcsProto.QoL;
using Leopotam.EcsProto.Unity;
using UnityEngine;
/*
public class RandomSpawnFurnitureSystem : IProtoInitSystem, IProtoRunSystem, IProtoDestroySystem
{
    private ProtoIt _iteratorPlayer;
    private ProtoIt _iteratorPlacement;
    private PlayerAspect _playerAspect;
    private PlacementAspect _placementAspect;
    private ProtoWorld _world;

    public void Init(IProtoSystems systems)
    {
        _world = systems.World();
        _playerAspect = (PlayerAspect)_world.Aspect(typeof(PlayerAspect));
        _placementAspect = (PlacementAspect)_world.Aspect(typeof(PlacementAspect));

        _iteratorPlayer = new(new[] { typeof(PlayerInputComponent), typeof(PositionComponent) });
        _iteratorPlacement = new(new[] { typeof(PlacementGridComponent) });

        _iteratorPlayer.Init(_world);
        _iteratorPlacement.Init(_world);
    }

    public void Run()
    {
        foreach (var entityPlayer in _iteratorPlayer)
        {
            ref var playerInput = ref _playerAspect.InputRawPool.Get(entityPlayer);
            if (!playerInput.RandomSpawnFurniturePressed) continue;

            Debug.Log("P была нажата");

            foreach (var entityGrid in _iteratorPlacement)
            {
                ref var gridComponent = ref _placementAspect.PlacementGridPool.Get(entityGrid);
                if (gridComponent.GridData is null) gridComponent.GridData = CreateEmptyWorld(gridComponent);

                bool isPlaced = false;
                for (int i = 0; i < gridComponent.GridSize.x; i++)
                {
                    for (int j = 0; j < gridComponent.GridSize.y; j++)
                    {
                        if (gridComponent.GridData[i][j].Type == FurnitureType.None)
                        {
                            var currentType = FurnitureType.Fridge; //заменить

                            var furniturePrefab = ProtoUnityLinks.Get(currentType.ToString()).Item1;
                            var furnitureObj = Object.Instantiate(furniturePrefab,
                                gridComponent.GridStartPosition + gridComponent.CellSize * new Vector3(i, j, 0),
                                Quaternion.identity);

                            //получаю сущность из авторинга
                            furnitureObj.GetComponent<ProtoUnityAuthoring>().Entity().TryUnpack(out var _, out var createdEntity);
                            ref var furnComponent = ref _placementAspect.FurniturePool.Get(createdEntity);
                            furnComponent.PositionInGrid = new Vector2Int(i, j);
                            gridComponent.GridData[i][j] = furnComponent;


                            isPlaced = true;
                            break;
                        }
                    }
                    if (isPlaced) break;
                }
                if (!isPlaced)
                {
                    Debug.Log("Свободного места на карте нет!!!");
                }
            }
        }
    }

    public void Destroy()
    {
        _iteratorPlayer = null;
    }

    private FurnitureComponent[][] CreateEmptyWorld(PlacementGridComponent grid)
    {
        grid.GridData = new FurnitureComponent[grid.GridSize.x][];
        for (int i = 0; i < grid.GridSize.x; i++)
        {
            grid.GridData[i] = new FurnitureComponent[grid.GridSize.y];
            for (int j = 0; j < grid.GridSize.y; j++)
            {
                grid.GridData[i][j] = new FurnitureComponent() { Type = FurnitureType.None };
            }
        }
        return grid.GridData;
    }
}
*/