using Game.Script.Aspects;
using Leopotam.EcsProto;
using Leopotam.EcsProto.QoL;
using UnityEngine;

namespace Game.Scripts.Systems
{
    public class GuestDestroyerSystem : IProtoInitSystem, IProtoRunSystem
    {
        [DI] private GuestAspect _guestAspect;
        private ProtoWorld _world;
        private ProtoIt _deadGuests;

        public void Init(IProtoSystems systems)
        {
            _world = systems.World();
            _deadGuests = new(new[] { typeof(GuestServicedTag), typeof(ReachedTargetPositionEvent) });
            _deadGuests.Init(_world);
        }

        public void Run()
        {
            foreach (var guest in _deadGuests)
            {
                Debug.Log("кремируйте её быстрее");
                ref var goRef = ref _guestAspect.GuestGameObjectRefComponentPool.Get(guest);
                if (goRef.GameObject)
                {
                    Object.Destroy(goRef.GameObject);
                    goRef.GameObject = null;
                }

                _world.DelEntity(guest);
            }
        }
    }
}