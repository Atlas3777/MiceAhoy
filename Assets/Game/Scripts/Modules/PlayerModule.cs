using System;
using Game.Script.Systems;
using Game.Scripts.Aspects;
using Leopotam.EcsProto;

class PlayerModule : IProtoModule
{
    public void Init(IProtoSystems systems)
    {
        systems
            .AddSystem(new TimerSystem())
            //.AddSystem(new PlayerInitializeInputSystem()) #TODO влключить лень
            //.AddSystem(new UpdateInputSystem()) 
            .AddSystem(new PlayerMovementSystem())
            .AddSystem(new PlayerTargetSystem());
    }

    public IProtoAspect[] Aspects()
    {
        return new IProtoAspect[] { new PlayerAspect(), new BaseAspect(), new ViewAspect() };
    }

    public Type[] Dependencies()
    {
        //return new[] { typeof(PhysicsModule) };
        return null;
    }
}