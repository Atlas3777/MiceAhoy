using Game.Script.Systems;
using Game.Scripts;
using Game.Scripts.Aspects;
using Game.Scripts.Infrastructure;
using Game.Scripts.Systems;
using Leopotam.EcsProto;
using Leopotam.EcsProto.ConditionalSystems;
using Leopotam.EcsProto.QoL;
using Leopotam.EcsProto.Unity;
using VContainer;
using System;
using UnityEngine;
using UnityEngine.LightTransport;

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
                    _r.Resolve<LevelProgresSystem>(),
                    _r.Resolve<WinLoseSystem>(),
                    
                    _r.Resolve<ClearSystem>())
                );

            return systems;
        }
    }
}