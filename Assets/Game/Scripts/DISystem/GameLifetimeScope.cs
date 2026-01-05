using Game.Script.Factories;
using Game.Script.Infrastructure;
using Game.Script.Input;
using Game.Script.Systems;
using Game.Scripts.Infrastructure;
using Game.Scripts.LevelSteps;
using Game.Scripts.Systems;
using Leopotam.EcsProto;
using Unity.Cinemachine;
using UnityEngine;
using VContainer;
using VContainer.Unity;
using Game.Scripts;

namespace Game.Script.DISystem
{
    public enum IProtoSystemsType
    {
        MainSystem,
        PhysicsSystem
    }

    public class GameLifetimeScope : LifetimeScope
    {
        [SerializeField] private LevelStepsList levelStepsList;
        [SerializeField] private PauseView pauseView;
        [SerializeField] private TutorialUIController tutorialUIController;
        [SerializeField] private CinemachineTargetGroup cinemachineTargetGroup;
        [SerializeField] private LevelConfig levelConfig;
        [SerializeField] private SpawnRegistry spawnPoint;

        protected override void Configure(IContainerBuilder builder)
        {
            Debug.Log("GameLifetimeScope : Configure");

            builder.RegisterEntryPoint<GameRuntimeController>();
            builder.Register<LevelFlowController>(Lifetime.Singleton).AsImplementedInterfaces().AsSelf();
            builder.Register<ManualPlayerSpawner>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.Register<InputService>(Lifetime.Singleton).AsImplementedInterfaces().AsSelf();

            builder.RegisterInstance<SpawnRegistry>(spawnPoint);
            
            builder.Register<GameResources>(Lifetime.Singleton);
            builder.Register<LevelStateService>(Lifetime.Singleton);
            builder.Register<RecipeService>(Lifetime.Singleton);
            builder.Register<PickableService>(Lifetime.Singleton);
            builder.Register<PlacementGrid>(Lifetime.Singleton);
            
            builder.Register<GameplaySolver>(Lifetime.Singleton);
            builder.Register<TutorialEcsPauseSolver>(Lifetime.Singleton);

            builder.RegisterInstance<LevelStepsList>(levelStepsList);
            builder.RegisterInstance<LevelConfig>(levelConfig);
            builder.RegisterComponent(cinemachineTargetGroup);
            builder.RegisterComponent(tutorialUIController);
            builder.RegisterComponent(pauseView);


            RegisterSystemFactories(builder);
            RegisterProtoSystems(builder);

            RegisterECSWorldAndSystems(builder);
        }

        private void RegisterECSWorldAndSystems(IContainerBuilder builder)
        {
            builder.Register<ECSWorldFactory>(Lifetime.Singleton);

            builder.Register<IProtoSystems>(container =>
                    container.Resolve<ECSWorldFactory>().CreateMainSystems(), Lifetime.Singleton)
                .Keyed(IProtoSystemsType.MainSystem);
        }

        private void RegisterSystemFactories(IContainerBuilder builder)
        {
            builder.Register<StoveSystemFactory>(Lifetime.Singleton);
            builder.Register<ItemSourceGeneratorSystemFactory>(Lifetime.Singleton);
            builder.Register<SyncUnityPhysicsToEcsSystemFactory>(Lifetime.Singleton);
            //builder.Register<PickPlaceSystemFactory>(Lifetime.Singleton);
            builder.Register<ClearSystemFactory>(Lifetime.Singleton);
            builder.Register<PlayerSpawnFurnitureSystemFactory>(Lifetime.Singleton);
            builder.Register<CreateGameObjectsSystemFactory>(Lifetime.Singleton);
            builder.Register<MoveFurnitureSystemFactory>(Lifetime.Singleton);
            builder.Register<MoveGameObjectSystemFactory>(Lifetime.Singleton);
            builder.Register<SyncGridPositionSystemFactory>(Lifetime.Singleton);
            builder.Register<RandomSpawnerPositionSystemFactory>(Lifetime.Singleton);
            builder.Register<DestroySpawnersSystemFactory>(Lifetime.Singleton);
            builder.Register<PlayerInitializeInputSystem>(Lifetime.Singleton);
            //builder.Register<EndGameSystemSystemFactory>(Lifetime.Singleton);
        }

        private void RegisterProtoSystems(IContainerBuilder builder)
        {
            builder.Register<PickPlaceSystem>(Lifetime.Singleton);
            builder.Register<UpdateInputSystem>(Lifetime.Singleton);
            builder.Register<ItemSourceGeneratorSystem>(Lifetime.Singleton);
            builder.Register<StoveSystem>(Lifetime.Singleton);
            //builder.Register<GroupGenerationSystem>(Lifetime.Singleton);
            builder.Register<ClearSystem>(Lifetime.Singleton);
            builder.Register<PlayerMovementSystem>(Lifetime.Singleton);
            builder.Register<SyncGridPositionSystem>(Lifetime.Singleton);
            builder.Register<GuestEatingSystem>(Lifetime.Singleton);
            builder.Register<LevelDirectorSystem>(Lifetime.Singleton);
            builder.Register<GuestSpawnSystem>(Lifetime.Singleton);
            builder.Register<ReputationSystem>(Lifetime.Singleton);
            builder.Register<WinGameSystem>(Lifetime.Singleton);
            builder.Register<LoseGameSystem>(Lifetime.Singleton);
            builder.Register<GuestNavigateToDestroySystem>(Lifetime.Singleton);
            

            // builder.RegisterFactory<ItemSourceGeneratorSystem>(container =>
            //     container.Resolve<ItemSourceGeneratorSystemFactory>().CreateProtoSystem, Lifetime.Singleton);

            // builder.RegisterFactory<StoveSystem>(container =>
            //     container.Resolve<StoveSystemFactory>().CreateProtoSystem, Lifetime.Singleton);

            builder.RegisterFactory<SyncUnityPhysicsToEcsSystem>(container =>
                container.Resolve<SyncUnityPhysicsToEcsSystemFactory>().CreateProtoSystem, Lifetime.Singleton);

            builder.RegisterFactory<PlayerInitializeInputSystem>(container =>
                container.Resolve<PlayerInitializeInputSystemFactory>().CreateProtoSystem, Lifetime.Singleton);

            // builder.RegisterFactory<PickPlaceSystem>(container =>
            //     container.Resolve<PickPlaceSystemFactory>().CreateProtoSystem, Lifetime.Singleton);

            // builder.RegisterFactory<ClearSystem>(container =>
            //     container.Resolve<ClearSystemFactory>().CreateProtoSystem, Lifetime.Singleton);

            // builder.RegisterFactory<EndGameSystem>(container =>
            //     container.Resolve<EndGameSystemSystemFactory>().CreateProtoSystem, Lifetime.Singleton);

            builder.RegisterFactory<PlayerSpawnFurnitureSystem>(container =>
                container.Resolve<PlayerSpawnFurnitureSystemFactory>().CreateProtoSystem, Lifetime.Singleton);

            builder.RegisterFactory<CreateGameObjectsSystem>(container =>
                container.Resolve<CreateGameObjectsSystemFactory>().CreateProtoSystem, Lifetime.Singleton);

            builder.RegisterFactory<MoveFurnitureSystem>(container =>
                container.Resolve<MoveFurnitureSystemFactory>().CreateProtoSystem, Lifetime.Singleton);

            builder.RegisterFactory<MoveGameObjectSystem>(container =>
                container.Resolve<MoveGameObjectSystemFactory>().CreateProtoSystem, Lifetime.Singleton);

            builder.RegisterFactory<SyncGridPositionSystem>(container =>
                container.Resolve<SyncGridPositionSystemFactory>().CreateProtoSystem, Lifetime.Singleton);

            builder.RegisterFactory<RandomSpawnerPositionSystem>(container =>
                container.Resolve<RandomSpawnerPositionSystemFactory>().CreateProtoSystem, Lifetime.Singleton);

            builder.RegisterFactory<DestroySpawnersSystem>(container =>
                container.Resolve<DestroySpawnersSystemFactory>().CreateProtoSystem, Lifetime.Singleton);
        }
    }
}