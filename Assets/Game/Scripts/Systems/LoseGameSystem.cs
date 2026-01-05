using System;
using Game.Script.Aspects;
using Game.Scripts.Aspects;
using Leopotam.EcsProto;
using Leopotam.EcsProto.QoL;
using UnityEngine;

public class LoseGameSystem : IProtoInitSystem, IProtoRunSystem
{
    [DI] GuestAspect _guestAspect;
    [DI] ProtoWorld _world;
    private ProtoIt _it;
    private ProtoIt _it2;
    private ProtoIt _itWin;

    public event Action EndGame;
    
    public void Init(IProtoSystems systems)
    {
        
    }

    public void Run()
    {
        
    }
}