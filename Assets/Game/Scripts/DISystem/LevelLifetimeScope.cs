using Game.Scripts.Infrastructure;
using Game.Scripts.Systems;
using Leopotam.EcsProto;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Game.Scripts.DISystem
{
    public class LevelLifetimeScope : LifetimeScope
    {
        private LevelConfig _levelConfig;
        private LevelContext _levelContext;
        
        public void SetConfig(LevelConfig config, LevelContext context)
        {
            _levelConfig = config;
            _levelContext = context;
        }

        protected override void Configure(IContainerBuilder builder)
        {
            Debug.Log("LevelLifetimeScope: Configure");
            builder.RegisterEntryPoint<LevelBootstrap>();
            
            builder.Register<LevelFlowController>(Lifetime.Singleton).AsImplementedInterfaces().AsSelf();
            builder.Register<LevelRuntimeController>(Lifetime.Singleton).AsImplementedInterfaces().AsSelf();
            
            builder.Register<PauseService>(Lifetime.Singleton).AsImplementedInterfaces().AsSelf();
            builder.Register<PausePresenter>(Lifetime.Singleton).AsImplementedInterfaces();
            
            builder.Register<LevelState>(Lifetime.Singleton);
            builder.Register<BuildModeService>(Lifetime.Singleton);
            
            builder.Register<GameplaySolver>(Lifetime.Singleton);
            builder.Register<EcsPauseSolver>(Lifetime.Singleton);
            
            builder.RegisterInstance(_levelConfig);
            builder.RegisterInstance(_levelContext);
            builder.RegisterInstance(_levelContext.positionsRegistry);

            RegisterProtoSystems(builder);
            

            builder.Register<IProtoSystems>(container =>
                _levelConfig.EcsSystemFactory.CreateSystems(container), Lifetime.Singleton);
        }

        private void RegisterProtoSystems(IContainerBuilder builder)
        {
            builder.Register<PickPlaceSystem>(Lifetime.Singleton);
            builder.Register<UpdateInputSystem>(Lifetime.Singleton);
            builder.Register<ItemSourceGeneratorSystem>(Lifetime.Singleton);
            builder.Register<StoveSystem>(Lifetime.Singleton);
            builder.Register<ClearSystem>(Lifetime.Singleton);
            builder.Register<PlayerMovementSystem>(Lifetime.Singleton);
            builder.Register<GuestEatingSystem>(Lifetime.Singleton);
            builder.Register<LevelDirectorSystem>(Lifetime.Singleton);
            builder.Register<ReputationSystem>(Lifetime.Singleton);
            builder.Register<WinLoseSystem>(Lifetime.Singleton);
            builder.Register<GuestNavigateToDestroySystem>(Lifetime.Singleton);
            builder.Register<LevelProgresSystem>(Lifetime.Singleton);
            builder.Register<PlayerInitializeInputSystem>(Lifetime.Singleton);
            builder.Register<GuestSpawnSystem>(Lifetime.Singleton);
            builder.Register<AngryGuestLeaveSystem>(Lifetime.Singleton);
            builder.Register<GuestDestroyerSystem>(Lifetime.Singleton);
            builder.Register<SoundSystem>(Lifetime.Singleton);
            builder.Register<MoveScrollMenuSystem>(Lifetime.Singleton);
            builder.Register<DestroySpawnersSystem>(Lifetime.Singleton);
            builder.Register<RandomSpawnerPositionSystem>(Lifetime.Singleton);
            builder.Register<SyncGridPositionSystem>(Lifetime.Singleton);
            builder.Register<PlayerSpawnFurnitureSystem>(Lifetime.Singleton);
            builder.Register<CreateGameObjectsSystem>(Lifetime.Singleton);
            builder.Register<MoveFurnitureSystem>(Lifetime.Singleton);
            builder.Register<MoveGameObjectSystem>(Lifetime.Singleton);
            builder.Register<StartDaySystem>(Lifetime.Singleton);
            builder.Register<PlayerPressedPSystem>(Lifetime.Singleton);
            builder.Register<PlayerPlacementSyncSystem>(Lifetime.Singleton);
            builder.Register<PlayerExitPlacementSystem>(Lifetime.Singleton);
            builder.Register<HappyGuestLeaveSystem>(Lifetime.Singleton);
        }
    }
}