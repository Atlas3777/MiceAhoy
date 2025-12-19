using System;
using Game.Script.Factories;
using Leopotam.EcsProto;
using UnityEngine;

public class PhysicsModule : IProtoModule
{
    private SyncUnityPhysicsToEcsSystem _syncUnityPhysicsToEcsSystem;

    public PhysicsModule(
        SyncUnityPhysicsToEcsSystemFactory syncUnityPhysicsToEcsSystemFactory)
    {
        _syncUnityPhysicsToEcsSystem = syncUnityPhysicsToEcsSystemFactory.CreateProtoSystem();
    }

    public void Init(IProtoSystems systems)
    {
        systems
            .AddSystem(_syncUnityPhysicsToEcsSystem);
        //.AddSystem(new UpdateItemViewPosition());
    }

    public IProtoAspect[] Aspects()
    {
        return new IProtoAspect[] { new PhysicsAspect() };
    }

    public Type[] Dependencies()
    {
        return null;
    }
}