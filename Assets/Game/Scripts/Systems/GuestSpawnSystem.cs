using System.Globalization;
using System.Linq;
using Game.Script.Aspects;
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
                ref var profile = ref _baseAspect.SpawnGuestRequestPool.Get(requestEntity).Profile;
                
                var go = Object.Instantiate(profile.Guest, _spawnPoint.position, Quaternion.identity);
                var agent = go.GetComponent<NavMeshAgent>();
                var r = go.GetComponent<CustomAuthoring>();
                r.ProcessAuthoring();
                var e = r.Entity();
                
                ref var guestStateComponent = ref e.GetOrAdd<GuestStateComponent>();
                ref var movementSpeedComponent = ref e.GetOrAdd<MovementSpeedComponent>();

                SetupView(e, profile);
                
                guestStateComponent.MaxHunger = profile.MaxHunger;
                guestStateComponent.Hunger = profile.MaxHunger;
                guestStateComponent.WaitingSeconds = profile.PatienceSeconds;
                movementSpeedComponent.Value = profile.MoveSpeed;
                
                agent.speed = movementSpeedComponent.Value;
                
                agent.updateRotation = false;
                agent.updateUpAxis = false;
                
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