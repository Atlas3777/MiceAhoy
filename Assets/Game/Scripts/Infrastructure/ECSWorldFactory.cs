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
                    
                .AddSystem(new SyncUnityPhysicsToEcsSystem())
                .AddSystem(new TimerSystem())
                //.AddSystem(new ProgressBarSystem())
                .AddSystem(_r.Resolve<SyncGridPositionSystem>())
                
                .AddSystem(_r.Resolve<PlayerInitializeInputSystem>())
                .AddSystem(_r.Resolve<UpdateInputSystem>())
                .AddSystem(_r.Resolve<PlayerMovementSystem>())
                .AddSystem(new PlayerTargetSystem())
                .AddSystem(new OutlineSystem())
                .AddSystem(_r.Resolve<PickPlaceSystem>())
                
                .AddSystem(_r.Resolve<StoveSystem>())
                .AddSystem(_r.Resolve<ItemSourceGeneratorSystem>())
                
                .AddSystem(new ConditionalSystem(_r.Resolve<GameplaySolver>(), true,
                    _r.Resolve<LevelDirectorSystem>(),
                    _r.Resolve<GuestSpawnSystem>())
                )
                
                .AddSystem(_r.Resolve<TableNotificationSystem>())
                //.AddSystem(new AcceptOrderSystem())
                .AddSystem(_r.Resolve<GuestGenerationSystem>())
                .AddSystem(new GuestGroupTableResolveSystem())
                .AddSystem(new GuestNavigateToTableSystem())
                .AddSystem(new GuestNavigateToDestroySystem(_r.Resolve<GameResources>()))
                .AddSystem(new GuestMovementSystem())
                //.AddSystem(new GroupArrivingRegistrySystem())
                .AddSystem(new GuestWaitingSystem())
                .AddSystem(new GuestDestroyerSystem())
                
                .AddSystem(_r.Resolve<ReputationSystem>())
                
                .AddSystem(_r.Resolve<WinGameSystem>())
                .AddSystem(_r.Resolve<LoseGameSystem>())
                
                .AddSystem(new PositionToTransformSystem()) //#TODO уничтожить
                .AddSystem(_r.Resolve<ClearSystem>(), 999);
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