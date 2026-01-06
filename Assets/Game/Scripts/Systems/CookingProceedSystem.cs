using Game.Scripts.Aspects;
using Leopotam.EcsProto;
using Leopotam.EcsProto.QoL;
using UnityEngine;

namespace Game.Scripts.Systems
{
    public class CookingProceedSystem : IProtoInitSystem, IProtoRunSystem
    {
        [DI] readonly WorkstationsAspect _workstationsAspect;
        [DI] readonly PlayerAspect _playerAspect;
        [DI] readonly BaseAspect _baseAspect;
        [DI] readonly ViewAspect _viewAspect;
        [DI] readonly PlacementAspect _placementAspect;
        [DI] readonly ProtoWorld _world;

        private ProtoIt _cookedWorkstations;
        
        public void Init(IProtoSystems systems)
        {
            _cookedWorkstations = new(new[] { typeof(ItemCookedTag) });
            _cookedWorkstations.Init(_world);
        }

        public void Run()
        {
            foreach (var workstationEntity in _cookedWorkstations)
            {
                _workstationsAspect.ItemPlaceEventPool.Add(workstationEntity);
                _workstationsAspect.ItemCookedTagPool.Del(workstationEntity);
                Debug.Log("Продолжаем готовку");
            }
        }
    }
}