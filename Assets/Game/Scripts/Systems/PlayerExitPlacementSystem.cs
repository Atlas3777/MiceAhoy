using Game.Scripts.Infrastructure;
using Leopotam.EcsProto;
using Leopotam.EcsProto.QoL;

namespace Game.Scripts.Systems
{
    public class PlayerExitPlacementSystem : IProtoInitSystem, IProtoRunSystem
    {
        [DI] readonly PlayerAspect _playerAspect;
        [DI] readonly PlacementAspect _placementAspect;
        private readonly BuildModeService _buildMode;

        private ProtoIt _playersInPlacement;

        public PlayerExitPlacementSystem(BuildModeService buildMode)
        {
            _buildMode = buildMode;
        }
        
        public void Init(IProtoSystems systems)
        {
            var world = systems.World();
            _playersInPlacement = new ProtoIt(
                new[] { typeof(PlayerInputComponent), typeof(PlacementModeTag) }
            );
            _playersInPlacement.Init(world);
        }

        public void Run()
        {
            if (_buildMode.IsBuildModeActive)
                return;

            foreach (var entity in _playersInPlacement)
            {
                ref var input = ref _playerAspect.InputRawPool.Get(entity);

                input.IsInPlacementMode = false;
                input.IsScrollMenuOpened = false;

                _placementAspect.PlacementModeTagPool.Del(entity);

                if (!_placementAspect.ActivateAllSpawnersEventPool.Has(entity))
                    _placementAspect.ActivateAllSpawnersEventPool.Add(entity);
            }
        }
    }
}