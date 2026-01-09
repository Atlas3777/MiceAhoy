using Game.Script.Factories;
using Leopotam.EcsProto;
using System;
using UnityEngine;

public class PlacementModule : IProtoModule
{
    private PlayerSpawnFurnitureSystem playerSpawnFurnitureSystem;
    private CreateGameObjectsSystem createGameObjectsSystem;
    private MoveFurnitureSystem moveFurnitureSystem;
    private MoveGameObjectSystem moveGameObjectSystem;
    private SyncGridPositionSystem syncGridPositionSystem;
    private RandomSpawnerPositionSystem randomSpawnerPositionSystem;
    private DestroySpawnersSystem destroySpawnersSystem;
    private MoveScrollMenuSystem moveScrollMenuSystem;

    public PlacementModule(PlayerSpawnFurnitureSystemFactory playerSpawnFurnitureSystem,
        CreateGameObjectsSystemFactory createGameObjectsSystem,
        MoveGameObjectSystemFactory moveGameObjectSystem,
        MoveFurnitureSystemFactory moveFurnitureSystem,
        SyncGridPositionSystemFactory syncGridPositionSystem,
        RandomSpawnerPositionSystemFactory randomSpawnerPositionSystem,
        DestroySpawnersSystemFactory destroySpawnersSystem,
        MoveScrollMenuSystemFactory moveScrollMenuSystem)
    {
        this.playerSpawnFurnitureSystem = playerSpawnFurnitureSystem.CreateProtoSystem();
        this.createGameObjectsSystem = createGameObjectsSystem.CreateProtoSystem();
        this.moveFurnitureSystem = moveFurnitureSystem.CreateProtoSystem();
        this.moveGameObjectSystem = moveGameObjectSystem.CreateProtoSystem();
        this.syncGridPositionSystem = syncGridPositionSystem.CreateProtoSystem();
        this.randomSpawnerPositionSystem = randomSpawnerPositionSystem.CreateProtoSystem();
        this.destroySpawnersSystem = destroySpawnersSystem.CreateProtoSystem();
        this.moveScrollMenuSystem = moveScrollMenuSystem.CreateProtoSystem();
    }

    public void Init(IProtoSystems systems)
    {
        systems
            .AddSystem(new PlayerPressedPSystem())
            .AddSystem(moveScrollMenuSystem)
            .AddSystem(playerSpawnFurnitureSystem)
            .AddSystem(createGameObjectsSystem)
            .AddSystem(moveFurnitureSystem)
            .AddSystem(moveGameObjectSystem)
            .AddSystem(syncGridPositionSystem)
            .AddSystem(randomSpawnerPositionSystem)
            .AddSystem(new SpawnerInteractSystem())
            .AddSystem(destroySpawnersSystem);        
    }

    public IProtoAspect[] Aspects()
    {
        return new IProtoAspect[] { new PlacementAspect() };
    }

    public Type[] Dependencies()
    {
        return null;
    }
}
