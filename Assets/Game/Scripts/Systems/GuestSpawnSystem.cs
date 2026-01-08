using System.Globalization;
using System.Linq;
using Game.Script.Aspects;
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
        [DI] GuestAspect _guestAspect;
        private ProtoIt _it;
        private readonly Transform _spawnPoint; 

        public GuestSpawnSystem(PositionsRegistry positionsRegistry)
        {
            _spawnPoint = positionsRegistry.GuestSpawn;
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

                if (request.Profile == null)
                {
                    Debug.LogError("SpawnGuestRequest with NULL Profile detected. Entity will be removed.");
                    _baseAspect.SpawnGuestRequestPool.Del(requestEntity);
                    continue;
                }
                var profile = request.Profile;

                
                var go = Object.Instantiate(profile.Guest, _spawnPoint.position, Quaternion.identity);
                var r = go.GetComponent<CustomAuthoring>();
                r.ProcessAuthoring();
                var e = r.Entity();
                
                ref var guestStateComponent = ref e.GetOrAdd<GuestStateComponent>();
                ref var movementSpeedComponent = ref e.GetOrAdd<MovementSpeedComponent>();
                ref var navMeshAgent = ref e.Get<NavMeshAgentComponent>().Agent;

                SetupView(e, profile);
                
                guestStateComponent.MaxHunger = profile.MaxHunger;
                guestStateComponent.Hunger = profile.MaxHunger;
                guestStateComponent.WaitingSeconds = profile.PatienceSeconds;
                guestStateComponent.ReputationLoss = profile.ReputationLoss;
                movementSpeedComponent.Value = profile.MoveSpeed;
                
                navMeshAgent.speed = movementSpeedComponent.Value;
                
                navMeshAgent.updateRotation = false;
                navMeshAgent.updateUpAxis = false;
                
                _baseAspect.SpawnGuestRequestPool.Del(requestEntity);
            }
        }

        public void SetupView(ProtoPackedEntityWithWorld e, GuestProfile guestProfile)
        {
            ref var vis = ref e.Get<GuestViewComponent>();
            vis.CurrentHunger.text = guestProfile.MaxHunger.ToString(CultureInfo.InvariantCulture);
        }
    }
}