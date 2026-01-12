using Game.Scripts.Systems;
using Leopotam.EcsProto;
using Leopotam.EcsProto.ConditionalSystems;
using Leopotam.EcsProto.QoL;
using Leopotam.EcsProto.Unity;
using VContainer;
using System;

namespace Game.Scripts.Infrastructure
{
    public abstract class EcsSystemsFactory
    {
        public abstract IProtoSystems CreateSystems(IObjectResolver resolver);
    }

    [Serializable]
    public class ECSMainSystemsFactory : EcsSystemsFactory
    {
        private IObjectResolver _r;

        public override IProtoSystems CreateSystems(IObjectResolver resolver)
        {
            _r = resolver;
            var world = resolver.Resolve<ProtoWorld>();
            var systems = BuildMainSystems(world);

            return systems;
        }


        private IProtoSystems BuildMainSystems(ProtoWorld world)
        {
            var systems = new ProtoSystems(world);

            systems
                .AddModule(new AutoInjectModule())
                .AddModule(new UnityModule())
                .AddSystem(new ConditionalSystem(_r.Resolve<EcsPauseSolver>(), true,
                    new SyncUnityPhysicsToEcsSystem(),
                    new TimerSystem(),
                    new ProgressBarSystem(),
                    _r.Resolve<SyncGridPositionSystem>(),

                    _r.Resolve<PlayerInitializeInputSystem>(),
                    _r.Resolve<UpdateInputSystem>(),
                    _r.Resolve<PlayerPlacementSyncSystem>(),
                    _r.Resolve<PlayerExitPlacementSystem>(),
                    _r.Resolve<PlayerMovementSystem>(),
                    new PlayerTargetSystem(),
                    new OutlineSystem(),
                    _r.Resolve<PickPlaceSystem>(),
                    _r.Resolve<StartDaySystem>(),
                    
                    new CookingProceedSystem(),
                    _r.Resolve<StoveSystem>(),

                    _r.Resolve<ItemSourceGeneratorSystem>(),

                    new ConditionalSystem(_r.Resolve<GameplaySolver>(), true,
                        _r.Resolve<LevelDirectorSystem>()
                    ),
                    _r.Resolve<GuestSpawnSystem>(),


                    new GuestBookTableSystem(),
                    new GuestNavigateToQueueSystem(_r.Resolve<LevelContext>().positionsRegistry.GuestsQueueHead),
                    new GuestNavigateToTableSystem(),
                    new GuestQueueTimeoutSystem(),
                    new MoveQueueSystem(_r.Resolve<LevelContext>().positionsRegistry.GuestsQueueHead),
                    new GuestMovementSystem(),
                    
                    new GuestWaitingSystem(),
                    //new QueueWaitingVisualizationSystem(),
                    _r.Resolve<GuestEatingSystem>(),
                    
                    _r.Resolve<HappyGuestLeaveSystem>(),
                    _r.Resolve<AngryGuestLeaveSystem>(),
                    
                    _r.Resolve<GuestNavigateToDestroySystem>(),
                    _r.Resolve<GuestDestroyerSystem>(),
                    
                    _r.Resolve<ReputationSystem>(),
                    _r.Resolve<LevelProgresSystem>(),
                    
                    _r.Resolve<SoundSystem>(),
                    _r.Resolve<WinLoseSystem>(),
                    
                    _r.Resolve<ClearSystem>(),

                    _r.Resolve<PlayerPressedPSystem>(),
                    _r.Resolve<MoveScrollMenuSystem>(),
                    _r.Resolve<PlayerSpawnFurnitureSystem>(),
                    _r.Resolve<CreateGameObjectsSystem>(),
                    _r.Resolve<MoveFurnitureSystem>(),
                    _r.Resolve<MoveGameObjectSystem>(),
                    _r.Resolve<SyncGridPositionSystem>(),
                    _r.Resolve<RandomSpawnerPositionSystem>(),
                    new SpawnerInteractSystem(),
                    _r.Resolve<DestroySpawnersSystem>())
                );

            return systems;
        }
        
    }
}