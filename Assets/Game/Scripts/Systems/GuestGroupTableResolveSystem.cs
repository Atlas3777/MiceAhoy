using System.Collections.Generic;
using Game.Script.Aspects;
using Leopotam.EcsProto;
using Leopotam.EcsProto.QoL;
using UnityEngine;

namespace Game.Script.Systems
{
    public class GuestGroupTableResolveSystem : IProtoInitSystem, IProtoRunSystem
    {
        [DI] private ProtoWorld _world;
        [DI] private GuestAspect _guestAspect;
        [DI] private WorkstationsAspect _workstationsAspect;
        [DI] private GuestGroupAspect _guestGroupAspect;
        [DI] private PhysicsAspect _physicsAspect;

        private ProtoIt _groupIterator;
        private ProtoIt _freeTablesIterator;

        public void Init(IProtoSystems systems)
        {
            _groupIterator = new(new[] { typeof(GuestGroupTag), typeof(GroupNeedsTableTag) });
            _freeTablesIterator = new(new[] { typeof(GuestTableComponent), typeof(GuestTableIsFreeTag) });
            _groupIterator.Init(_world);
            _freeTablesIterator.Init(_world);
        }

        public void Run()
        {
            foreach (var guestGroupEntity in _groupIterator)
            {
                Debug.Log("поиск");
                foreach (var tableEntity in _freeTablesIterator)
                {
                    Debug.Log("стол существует");
                    ref var group = ref _guestGroupAspect.GuestGroupPool.Get(guestGroupEntity);
                    ref var table = ref _workstationsAspect.GuestTablePool.Get(tableEntity);
                    group.table = _world.PackEntityWithWorld(tableEntity);
                    table.Guest = _world.PackEntityWithWorld(guestGroupEntity);
                    _guestAspect.GuestTableIsFreeTagPool.Del(tableEntity);
                    _guestGroupAspect.GroupNeedsTablePool.Del(guestGroupEntity);
                    _guestGroupAspect.GroupGotTableEventPool.Add(guestGroupEntity);
                    _guestGroupAspect.GroupIsWalkingPool.Add(guestGroupEntity);

                    ref var groupPos = ref _physicsAspect.PositionPool.Get(guestGroupEntity);
                    groupPos.Position = _physicsAspect.PositionPool.Get(tableEntity).Position;
                    Debug.Log("Выдали группе стол");
                    break;
                }
            }
        }
    }
}