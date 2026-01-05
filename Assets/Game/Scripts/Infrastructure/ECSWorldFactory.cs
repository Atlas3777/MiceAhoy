using Game.Script.Systems;
using Game.Scripts;
using Game.Scripts.Aspects;
using Game.Scripts.Systems;
using Leopotam.EcsProto;
using Leopotam.EcsProto.ConditionalSystems;
using Leopotam.EcsProto.QoL;
using Leopotam.EcsProto.Unity;
using VContainer;

namespace Game.Script.Infrastructure
{
    public class ECSWorldFactory
    {
        private readonly IObjectResolver _r;

        public ECSWorldFactory(IObjectResolver resolver)
            => _r = resolver;

        public IProtoSystems CreateMainSystems()
        {
            var world = new ProtoWorld(new BaseAspect());
            var systems = BuildMainSystems(world);

            return systems;
        }


        private IProtoSystems BuildMainSystems(ProtoWorld world)
        {
            var systems = new ProtoSystems(world);

            systems
                .AddModule(new AutoInjectModule())
                .AddModule(new UnityModule())
                
                .AddSystem(new ConditionalSystem(_r.Resolve<TutorialEcsPauseSolver>(), true,
                    
                    new SyncUnityPhysicsToEcsSystem(),
                    new TimerSystem(),
                    new ProgressBarSystem(),
                    _r.Resolve<SyncGridPositionSystem>(),
                    
                    _r.Resolve<PlayerInitializeInputSystem>(),
                    _r.Resolve<UpdateInputSystem>(),
                    _r.Resolve<PlayerMovementSystem>(),
                    new PlayerTargetSystem(),
                    new OutlineSystem(),
                    _r.Resolve<PickPlaceSystem>(),
                    
                    _r.Resolve<StoveSystem>(),
                    _r.Resolve<ItemSourceGeneratorSystem>(),
                    
                    new ConditionalSystem(_r.Resolve<GameplaySolver>(), true,
                        _r.Resolve<LevelDirectorSystem>()
                    ),
                    _r.Resolve<GuestSpawnSystem>(),
                    
                    
                    new GuestBookTableSystem(),
                    new GuestNavigateToTableSystem(),
                    new GuestMovementSystem(),
                    
                    new GuestWaitingSystem(),
                    _r.Resolve<GuestEatingSystem>(),
                    
                    new HappyGuestLeaveSystem(),
                    new AngryGuestLeaveSystem(),
                    
                    _r.Resolve<GuestNavigateToDestroySystem>(),
                    new GuestDestroyerSystem(),
                    
                    _r.Resolve<ReputationSystem>(),
                    
                    _r.Resolve<WinGameSystem>(),
                    _r.Resolve<LoseGameSystem>(),
                    
                    _r.Resolve<ClearSystem>())
                );

            return systems;
        }
    }
}


// .AddSystem(playerSpawnFurnitureSystem) 
// .AddSystem(createGameObjectsSystem)
// .AddSystem(moveFurnitureSystem)
// .AddSystem(moveGameObjectSystem)
// .AddSystem(syncGridPositionSystem)
// .AddSystem(randomSpawnerPositionSystem)
// .AddSystem(new SpawnerInteractSystem())
// .AddSystem(destroySpawnersSystem)