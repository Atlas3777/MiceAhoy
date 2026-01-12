using Leopotam.EcsProto;
using Leopotam.EcsProto.QoL;

namespace Game.Scripts.Infrastructure
{
    public class PlayerPlacementSyncSystem : IProtoRunSystem
    {
        [DI] readonly PlayerAspect _playerAspect;
        [DI] readonly PlacementAspect _placementAspect;
        private readonly BuildModeService _buildMode;
        
        public PlayerPlacementSyncSystem(BuildModeService buildMode)
        {
            _buildMode = buildMode;
        }

        public void Run()
        {
            foreach (var entity in _playerAspect.PlayerInitializeEventPool)
            {
                if (!_buildMode.IsBuildModeActive)
                    continue;

                ref var input = ref _playerAspect.InputRawPool.Get(entity);
                input.IsInPlacementMode = true;

                if (!_placementAspect.PlacementModeTagPool.Has(entity))
                    _placementAspect.PlacementModeTagPool.Add(entity);
            }
        }
    }
}