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
        private readonly EndGameSystem _endGameSystem;
        public GuestModule(GroupGenerationSystemFactory groupGenerationSystemFactory, EndGameSystem  endGameSystem)
        {
            this._groupGenerationSystem = groupGenerationSystemFactory.CreateProtoSystem();
            _endGameSystem = endGameSystem;
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
                .AddSystem(_endGameSystem)
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
