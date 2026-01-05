using System;
using Game.Script.Aspects;
using Game.Script.Factories;
using Game.Script.Systems;
using Leopotam.EcsProto;

namespace Game.Script.Modules
{
    public class GuestModule : IProtoModule
    {
        private readonly GroupGenerationSystem _groupGenerationSystem;
        private readonly LoseGameSystem _loseGameSystem;
        public GuestModule(GroupGenerationSystemFactory groupGenerationSystemFactory, LoseGameSystem  loseGameSystem)
        {
            this._groupGenerationSystem = groupGenerationSystemFactory.CreateProtoSystem();
            _loseGameSystem = loseGameSystem;
        }
        
        public void Init(IProtoSystems systems)
        {
            systems
                .AddSystem(_groupGenerationSystem)
                .AddSystem(new GuestGroupTableResolveSystem())
                .AddSystem(new GuestNavigateToTableSystem())
                .AddSystem(new GuestMovementSystem())
                .AddSystem(new GroupArrivingRegistrySystem())
                .AddSystem(new GuestWaitingSystem())
                .AddSystem(new GuestDestroyerSystem())
                .AddSystem(_loseGameSystem)
                .AddSystem(new GuestNavigateToDestroySystem())
                .AddSystem(new PositionToTransformSystem());
        }

        public IProtoAspect[] Aspects()
        {
            return new IProtoAspect[] { new GuestAspect(), new GuestGroupAspect() };
        }

        public Type[] Dependencies()
        {
            return null;
        }
    }
}
