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
            
            builder.Register<RuntimeLevelState>(Lifetime.Singleton);
            
            builder.Register<GameplaySolver>(Lifetime.Singleton);
            builder.Register<EcsPauseSolver>(Lifetime.Singleton);
            
            builder.RegisterInstance(_levelConfig);
            builder.RegisterInstance(_levelContext);
            builder.RegisterInstance(_levelContext.spawnRegistry);

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
            builder.Register<SyncGridPositionSystem>(Lifetime.Singleton);
            builder.Register<GuestEatingSystem>(Lifetime.Singleton);
            builder.Register<LevelDirectorSystem>(Lifetime.Singleton);
            builder.Register<ReputationSystem>(Lifetime.Singleton);
            builder.Register<WinLoseSystem>(Lifetime.Singleton);
            builder.Register<GuestNavigateToDestroySystem>(Lifetime.Singleton);
            builder.Register<LevelProgresSystem>(Lifetime.Singleton);
            builder.Register<PlayerInitializeInputSystem>(Lifetime.Singleton);
            builder.Register<GuestSpawnSystem>(Lifetime.Singleton);
        }
    }
}