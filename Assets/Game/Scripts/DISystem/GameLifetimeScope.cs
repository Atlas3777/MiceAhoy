using Game.Script.Factories;
using Game.Script.Infrastructure;
using Game.Script.Modules;
using Game.Script.Systems;
using Leopotam.EcsProto;
using UnityEngine;
using UnityEngine.Playables;
using VContainer;
using VContainer.Unity;

namespace Game.Script.DISystem
{
    public enum IProtoSystemsType
    {
        MainSystem,
        PhysicsSystem
    }

    public class GameLifetimeScope : LifetimeScope
    {
        [SerializeField] private Grid grid;
        [SerializeField] private PlayableDirector playableDirector;
        [SerializeField] private InputService inputService;
        [SerializeField] private UIController uiController;

        protected override void Configure(IContainerBuilder builder)
        {
            Debug.Log("1111111111111111");
            builder.Register<SceneController>(Lifetime.Singleton);
            builder.RegisterEntryPoint<GameStateManager>();
            Debug.Log("rfrjsfhdsazklfh");
            builder.Register<GameResources>(Lifetime.Singleton);
            builder.Register<RecipeService>(Lifetime.Singleton);
            builder.Register<PickableService>(Lifetime.Singleton);
            
            builder.RegisterComponent(playableDirector).AsSelf();
            builder.RegisterComponent(uiController).AsSelf();
            builder.RegisterComponent(inputService).AsSelf();
            builder.RegisterComponent(grid);
            
            builder.Register<PlacementGrid>(Lifetime.Singleton);
            
            RegisterSystemFactories(builder);
            RegisterProtoSystems(builder);
            RegisterModules(builder);
            RegisterECSWorldAndSystems(builder);
        }
        
        private void RegisterECSWorldAndSystems(IContainerBuilder builder)
        {
            builder.Register<MainGameECSWorldFactory>(Lifetime.Singleton);

            builder.Register<IProtoSystems>(container =>
                    container.Resolve<MainGameECSWorldFactory>().MainSystemsECSFactory(), Lifetime.Singleton)
                .Keyed(IProtoSystemsType.MainSystem);

            builder.Register<IProtoSystems>(container =>
                    container.Resolve<MainGameECSWorldFactory>().PhysicsSystemsECSFactory(), Lifetime.Singleton)
                .Keyed(IProtoSystemsType.PhysicsSystem);
        }
        
        private void RegisterModules(IContainerBuilder builder)
        {
            builder.Register<WorkstationsModule>(Lifetime.Singleton);
            builder.Register<PlacementModule>(Lifetime.Singleton);
            builder.Register<PhysicsModule>(Lifetime.Singleton);
            builder.Register<GuestModule>(Lifetime.Singleton);
        }

        private void RegisterSystemFactories(IContainerBuilder builder)
        {
            builder.Register<StoveSystemFactory>(Lifetime.Singleton);
            builder.Register<ItemSourceGeneratorSystemFactory>(Lifetime.Singleton);
            builder.Register<SyncUnityPhysicsToEcsSystemFactory>(Lifetime.Singleton);
            builder.Register<PickPlaceSystemFactory>(Lifetime.Singleton);
            builder.Register<ClearSystemFactory>(Lifetime.Singleton);
            builder.Register<PlayerSpawnFurnitureSystemFactory>(Lifetime.Singleton);
            builder.Register<CreateGameObjectsSystemFactory>(Lifetime.Singleton);
            builder.Register<MoveFurnitureSystemFactory>(Lifetime.Singleton);
            builder.Register<MoveGameObjectSystemFactory>(Lifetime.Singleton);
            builder.Register<SyncGridPositionSystemFactory>(Lifetime.Singleton);
            builder.Register<GroupGenerationSystemFactory>(Lifetime.Singleton);
            builder.Register<RandomSpawnerPositionSystemFactory>(Lifetime.Singleton);
            builder.Register<DestroySpawnersSystemFactory>(Lifetime.Singleton);
            //builder.Register<EndGameSystemSystemFactory>(Lifetime.Singleton);
        }

        private void RegisterProtoSystems(IContainerBuilder builder)
        {
            builder.Register<EndGameSystem>(Lifetime.Singleton);

            builder.RegisterFactory<ItemSourceGeneratorSystem>(container =>
                container.Resolve<ItemSourceGeneratorSystemFactory>().CreateProtoSystem, Lifetime.Singleton);

            builder.RegisterFactory<StoveSystem>(container =>
                container.Resolve<StoveSystemFactory>().CreateProtoSystem, Lifetime.Singleton);

            builder.RegisterFactory<SyncUnityPhysicsToEcsSystem>(container =>
                container.Resolve<SyncUnityPhysicsToEcsSystemFactory>().CreateProtoSystem, Lifetime.Singleton);

            builder.RegisterFactory<PickPlaceSystem>(container =>
                container.Resolve<PickPlaceSystemFactory>().CreateProtoSystem, Lifetime.Singleton);

            builder.RegisterFactory<ClearSystem>(container =>
                container.Resolve<ClearSystemFactory>().CreateProtoSystem, Lifetime.Singleton);

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

            builder.RegisterFactory<GroupGenerationSystem>(container =>
                container.Resolve<GroupGenerationSystemFactory>().CreateProtoSystem, Lifetime.Singleton);

            builder.RegisterFactory<RandomSpawnerPositionSystem>(container =>
                container.Resolve<RandomSpawnerPositionSystemFactory>().CreateProtoSystem, Lifetime.Singleton);

            builder.RegisterFactory<DestroySpawnersSystem>(container =>
                container.Resolve<DestroySpawnersSystemFactory>().CreateProtoSystem, Lifetime.Singleton);
        }
    }
}