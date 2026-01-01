using System.Collections.Generic;
using Game.Script.Aspects;
using Leopotam.EcsProto;
using Leopotam.EcsProto.QoL;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

namespace Game.Script.Systems
{
    public class GroupGenerationSystem : IProtoInitSystem, IProtoRunSystem
    {
        [DI] private GuestGroupAspect _guestGroupAspect;
        [DI] private GuestAspect _guestAspect;
        [DI] private ProtoWorld _world;
        
        private readonly GameObject _guestPrefab;
        private ProtoIt _groupsToGenerateIterator;
        
        public GroupGenerationSystem(GameObject guestPrefab)
        {
            this._guestPrefab = guestPrefab;
        }

        public void Init(IProtoSystems systems)
        {
            _groupsToGenerateIterator = new(new[] { typeof(GuestGroupTag), typeof(TargetGroupSize) });
            _groupsToGenerateIterator.Init(_world);
        }

        public void Run()
        {
            foreach (var groupEntity in _groupsToGenerateIterator)
            {
                Debug.Log("создаём гостей");
                var numberOfGuests = _guestGroupAspect.TargetGroupSizePool.Get(groupEntity).size;
                var guests = CreateGuests(numberOfGuests);
                foreach (var packed in guests)
                {
                    if (packed.TryUnpack(out _, out var guestEntity))
                    {
                        ref var groupComp = ref _guestAspect.GuestGroupComponentPool.Add(guestEntity);
                        groupComp.GuestGroup = _world.PackEntityWithWorld(groupEntity);
                    }
                }
                ref var groupGuests = ref _guestGroupAspect.GuestGroupPool.Get(groupEntity);
                groupGuests.includedGuests = guests;
                _guestGroupAspect.TargetGroupSizePool.Del(groupEntity);
            }
        }
        
        private List<ProtoPackedEntityWithWorld> CreateGuests(int numberOfGuests = 1)
        {
            var guests = new List<ProtoPackedEntityWithWorld>();
            for (var i = 0; i < numberOfGuests; ++i)
            {
                var go = Object.Instantiate(_guestPrefab);
                var authoring = go.GetComponent<CustomAuthoring>();

                authoring.ProcessAuthoring();
                var entity = authoring.Entity();
                entity.TryUnpack(out _, out var unpackedEntity);
                
                ref var goRef = ref _guestAspect.GuestGameObjectRefComponentPool.Add(unpackedEntity);
                goRef.GameObject = go;
                
                var agent = go.GetComponent<NavMeshAgent>();
                ref var agentComponent = ref _guestAspect.NavMeshAgentComponentPool.Add(unpackedEntity);
                agentComponent.Agent = agent;
                agent.updateRotation = false;
                agent.updateUpAxis = false;

                guests.Add(entity);
            }
            return guests;
        }
    }
}