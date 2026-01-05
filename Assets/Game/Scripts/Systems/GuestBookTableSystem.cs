using System.Collections.Generic;
using Game.Script.Aspects;
using Leopotam.EcsProto;
using Leopotam.EcsProto.QoL;
using UnityEngine;

namespace Game.Script.Systems
{
    public class GuestBookTableSystem : IProtoInitSystem, IProtoRunSystem
    {
        [DI] private ProtoWorld _world;
        [DI] private GuestAspect _guestAspect;
        [DI] private WorkstationsAspect _workstationsAspect;
        [DI] private PhysicsAspect _physicsAspect;

        private ProtoIt _guestIterator;
        private ProtoIt _freeTablesIterator;

        public void Init(IProtoSystems systems)
        {
            _guestIterator = new(new[] { typeof(GuestTag), typeof(NeedsTableTag) });
            _freeTablesIterator = new(new[] { typeof(GuestTableComponent), typeof(GuestTableIsFreeTag) });
            _guestIterator.Init(_world);
            _freeTablesIterator.Init(_world);
        }

        public void Run()
        {
            foreach (var guestEntity in _guestIterator)
            {
                if (!_guestAspect.NeedsTableTagPool.Has(guestEntity)) continue;
                Debug.Log(guestEntity);
                foreach (var tableEntity in _freeTablesIterator)
                {
                    if (!_guestAspect.GuestTableIsFreeTagPool.Has(tableEntity)) continue;
                    Debug.Log("стол существует");
                    ref var guest = ref _guestAspect.TargetPositionComponentPool.Get(guestEntity);
                    ref var table = ref _workstationsAspect.GuestTablePool.Get(tableEntity);
                    guest.Table = _world.PackEntityWithWorld(tableEntity);
                    table.Guest = _world.PackEntityWithWorld(guestEntity);
                    _guestAspect.GuestTableIsFreeTagPool.Del(tableEntity);
                    _guestAspect.NeedsTableTagPool.Del(guestEntity);
                    _guestAspect.GotTableEventPool.Add(guestEntity);
                    Debug.LogWarning(_guestAspect.NeedsTableTagPool.Has(guestEntity));
                    _guestAspect.GuestIsWalkingTagPool.Add(guestEntity);

                    ref var groupPos = ref _physicsAspect.PositionPool.Get(guestEntity);
                    groupPos.Position = _physicsAspect.PositionPool.Get(tableEntity).Position;
                    Debug.Log("Выдали стол");
                    break;
                }
            }
        }
    }
}