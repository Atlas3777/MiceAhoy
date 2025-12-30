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

        public ECSWorldFactory(IObjectResolver r)
        {
            _r = r;
        }

        /// <summary>
        /// Создаёт и возвращает основные системы игры. Всё в одном месте — полный пайплайн обработки.
        /// </summary>
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
            if (_world != null) return;

            _world = new ProtoWorld(new BaseAspect());
        }

        private void BuildMainSystems()
        {
            _systems = new ProtoSystems(_world);

            // Разрешаем фабрики и создаём системы
            var groupGenerationSystem = _r.Resolve<GroupGenerationSystemFactory>().CreateProtoSystem();

            var playerSpawnFurnitureSystem = _r.Resolve<PlayerSpawnFurnitureSystemFactory>().CreateProtoSystem();
            var createGameObjectsSystem = _r.Resolve<CreateGameObjectsSystemFactory>().CreateProtoSystem();
            var moveFurnitureSystem = _r.Resolve<MoveFurnitureSystemFactory>().CreateProtoSystem();
            var moveGameObjectSystem = _r.Resolve<MoveGameObjectSystemFactory>().CreateProtoSystem();
            var syncGridPositionSystem = _r.Resolve<SyncGridPositionSystemFactory>().CreateProtoSystem();
            var randomSpawnerPositionSystem = _r.Resolve<RandomSpawnerPositionSystemFactory>().CreateProtoSystem();
            var destroySpawnersSystem = _r.Resolve<DestroySpawnersSystemFactory>().CreateProtoSystem();

            var itemSourceGeneratorSystem = _r.Resolve<ItemSourceGeneratorSystemFactory>().CreateProtoSystem();
            var stoveSystem = _r.Resolve<StoveSystemFactory>().CreateProtoSystem();
            var clearSystem = _r.Resolve<ClearSystemFactory>().CreateProtoSystem();
            var endGameSystem = _r.Resolve<EndGameSystem>();
            var pickPlaceSystem = _r.Resolve<PickPlaceSystem>();
            
            
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
                .AddSystem(pickPlaceSystem) 
                
                .AddSystem(new GuestTableSetupSystem()) 
                .AddSystem(new TableNotificationSystem())
                .AddSystem(new AcceptOrderSystem())
                .AddSystem(itemSourceGeneratorSystem)
                .AddSystem(stoveSystem)
                
                // .AddSystem(playerSpawnFurnitureSystem) 
                // .AddSystem(createGameObjectsSystem)
                // .AddSystem(moveFurnitureSystem)
                // .AddSystem(moveGameObjectSystem)
                // .AddSystem(syncGridPositionSystem)
                // .AddSystem(randomSpawnerPositionSystem)
                // .AddSystem(new SpawnerInteractSystem())
                // .AddSystem(destroySpawnersSystem)
                
                .AddSystem(groupGenerationSystem)
                .AddSystem(new GuestGroupTableResolveSystem())
                .AddSystem(new GuestNavigateToTableSystem())
                .AddSystem(new GuestMovementSystem())
                .AddSystem(new GroupArrivingRegistrySystem())
                .AddSystem(new GuestWaitingSystem())
                .AddSystem(new GuestNavigateToDestroySystem())
                .AddSystem(new GuestDestroyerSystem())
                
                .AddSystem(endGameSystem)
                .AddSystem(new PositionToTransformSystem()) 
                .AddSystem(clearSystem, 999);
        }
    }
}