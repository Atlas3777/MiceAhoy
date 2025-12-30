using System;
using Game.Script.Factories;
using Game.Script.Systems;
using Leopotam.EcsProto;
using UnityEngine;

public class WorkstationsModule : IProtoModule
{
    ItemSourceGeneratorSystem _itemSourceGeneratorSystem;
    StoveSystem _stoveSystem;
    PickPlaceSystem _pickPlaceSystem;
    ClearSystem _clearSystem;

    public WorkstationsModule(ItemSourceGeneratorSystemFactory itemSourceGeneratorSystemFactory,
        StoveSystemFactory stoveSystemFactory,
        PickPlaceSystemFactory pickPlaceSystemFactory,
        ClearSystemFactory clearSystemFactory
        )
    {
        _itemSourceGeneratorSystem = itemSourceGeneratorSystemFactory.CreateProtoSystem();
        _stoveSystem = stoveSystemFactory.CreateProtoSystem();
        _pickPlaceSystem = pickPlaceSystemFactory.CreateProtoSystem();
        _clearSystem = clearSystemFactory.CreateProtoSystem();
    }
    
    public void Init(IProtoSystems systems)
    {
        systems
            .AddSystem(_pickPlaceSystem)
            .AddSystem(new GuestTableSetupSystem())
            .AddSystem(new AcceptOrderSystem())
            .AddSystem(new TableNotificationSystem())
            .AddSystem(_itemSourceGeneratorSystem)
            .AddSystem(_stoveSystem)
            .AddSystem(new ProgressBarSystem())
            .AddSystem(_clearSystem, 999);
    }

    public IProtoAspect[] Aspects()
    {
        return new IProtoAspect[] { new WorkstationsAspect() };
    }

    public Type[] Dependencies()
    {
        return null;
    }
}