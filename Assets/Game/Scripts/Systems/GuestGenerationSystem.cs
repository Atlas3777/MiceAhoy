using System.Collections.Generic;
using Game.Script.Aspects;
using Leopotam.EcsProto;
using Leopotam.EcsProto.QoL;
using UnityEngine;
using UnityEngine.AI;

namespace Game.Script.Systems
{
    public class GuestGenerationSystem : IProtoInitSystem, IProtoRunSystem
    {
        [DI] private GuestAspect _guestAspect;
        [DI] private ProtoWorld _world;
        
        private readonly GameObject _guestPrefab;
        private ProtoIt _guestToGenerateIterator;
        
        public GuestGenerationSystem(GameObject guestPrefab)
        {
            this._guestPrefab = guestPrefab;
        }

        public void Init(IProtoSystems systems)
        {
            _guestToGenerateIterator = new(new[] { typeof(GuestRequestEvent) });
            _guestToGenerateIterator.Init(_world);
        }

        public void Run()
        {
            // foreach (var guest in _guestToGenerateIterator)
            // {
            //     Debug.Log("создаём гостя");
            //     CreateGuests();
            //     _guestAspect.GuestRequestEventPool.Del(guest);
            // }
        }
        
        private List<ProtoPackedEntityWithWorld> CreateGuests(int numberOfGuests = 1)
        {
            Debug.LogError("рот ебал мамаши автора ecs");
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