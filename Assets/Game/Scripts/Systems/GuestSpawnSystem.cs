using Game.Script.DISystem;
using Game.Scripts.Aspects;
using Leopotam.EcsProto;
using Leopotam.EcsProto.QoL;
using UnityEngine;
using UnityEngine.AI;

namespace Game.Scripts.Systems
{
    public class GuestSpawnSystem : IProtoInitSystem, IProtoRunSystem
    {
        [DI] ProtoWorld _world;
        [DI] BaseAspect _baseAspect;
        private ProtoIt _it;
        private readonly Transform _spawnPoint; 

        public GuestSpawnSystem(SpawnRegistry spawnRegistry)
        {
            _spawnPoint = spawnRegistry.GuestSpawn;
        }
        
        public void Init(IProtoSystems systems)
        {
            _it = new(new []{typeof(SpawnGuestRequest)});
            _it.Init(_world);
        }

        public void Run()
        {
            foreach (var requestEntity in _it)
            {
                ref var request = ref _baseAspect.SpawnGuestRequestPool.Get(requestEntity);
                
                var go = Object.Instantiate(request.Profile.Guest, _spawnPoint.position, Quaternion.identity);
                var agent = go.GetComponent<NavMeshAgent>();
                agent.updateRotation = false;
                agent.updateUpAxis = false;
                
                _baseAspect.SpawnGuestRequestPool.Del(requestEntity);
            }
        }
    }
}