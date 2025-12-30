using Game.Script.Factories;
using Game.Script.Systems;
using Leopotam.EcsProto;
using Leopotam.EcsProto.QoL;
using Leopotam.EcsProto.Unity;
using VContainer;

namespace Game.Script.Infrastructure
{
    public class ECSWorldFactory
    {
        private ProtoWorld _world;
        private ProtoSystems _systems;

        private readonly IObjectResolver _r;

        public ECSWorldFactory(IObjectResolver resolver)
            => _r = resolver;
        

        public IProtoSystems CreateMainSystems()
        {
            if (_systems != null)
                return _systems;

            BuildWorld();
            BuildMainSystems();

            return _systems;
        }

        private void BuildWorld()
        {
            if (_world != null)
                _world = new ProtoWorld(new BaseAspect());
        }

        private void BuildMainSystems()
        {
            _systems = new ProtoSystems(_world);
            
            _systems
                .AddModule(new AutoInjectModule())
                .AddModule(new UnityModule())
                    
                .AddSystem(new SyncUnityPhysicsToEcsSystem())
                .AddSystem(new TimerSystem())
                .AddSystem(new ProgressBarSystem())
                
                .AddSystem(_r.Resolve<PlayerInitializeInputSystem>())
                .AddSystem(_r.Resolve<UpdateInputSystem>())
                .AddSystem(new PlayerMovementSystem())
                .AddSystem(new PlayerTargetSystem())
                .AddSystem(_r.Resolve<PickPlaceSystem>()) 
                
                .AddSystem(new GuestTableSetupSystem()) 
                .AddSystem(new TableNotificationSystem())
                .AddSystem(new AcceptOrderSystem())
                .AddSystem(_r.Resolve<ItemSourceGeneratorSystem>())
                .AddSystem(_r.Resolve<StoveSystem>())
                
                .AddSystem(_r.Resolve<GroupGenerationSystem>())
                .AddSystem(new GuestGroupTableResolveSystem())
                .AddSystem(new GuestNavigateToTableSystem())
                .AddSystem(new GuestMovementSystem())
                .AddSystem(new GroupArrivingRegistrySystem())
                .AddSystem(new GuestWaitingSystem())
                .AddSystem(new GuestNavigateToDestroySystem())
                .AddSystem(new GuestDestroyerSystem())
                
                .AddSystem(_r.Resolve<EndGameSystem>())
                .AddSystem(new PositionToTransformSystem()) 
                .AddSystem(_r.Resolve<ClearSystem>(), 999);
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