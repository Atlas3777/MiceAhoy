using System;
using Game.Script.Aspects;
using Game.Script.Factories;
using Game.Script.Systems;
using Leopotam.EcsProto;

namespace Game.Script.Modules
{
    public class GuestModule : IProtoModule
    {
        private readonly GuestGenerationSystem _guestGenerationSystem;
        private readonly GuestServicingFinalSystem _guestServicingFinalSystem;
        public GuestModule(GroupGenerationSystemFactory groupGenerationSystemFactory, GuestServicingFinalSystem  guestServicingFinalSystem)
        {
            this._guestGenerationSystem = groupGenerationSystemFactory.CreateProtoSystem();
            _guestServicingFinalSystem = guestServicingFinalSystem;
        }
        
        public void Init(IProtoSystems systems)
        {
            systems
                .AddSystem(_guestGenerationSystem)
                .AddSystem(new GuestGroupTableResolveSystem())
                .AddSystem(new GuestNavigateToTableSystem())
                .AddSystem(new GuestMovementSystem())
                //.AddSystem(new GroupArrivingRegistrySystem())
                .AddSystem(new GuestWaitingSystem())
                .AddSystem(new GuestDestroyerSystem())
                .AddSystem(_guestServicingFinalSystem)
                //.AddSystem(new GuestNavigateToDestroySystem()) TODO: ПОХУЙ
                .AddSystem(new PositionToTransformSystem());
        }

        public IProtoAspect[] Aspects()
        {
            return new IProtoAspect[] { new GuestAspect(), /*new GuestGroupAspect()*/ };
        }

        public Type[] Dependencies()
        {
            return null;
        }
    }
}
