using Game.Scripts.Aspects;
using Game.Scripts.Infrastructure;
using Leopotam.EcsProto;
using Leopotam.EcsProto.QoL;

namespace Game.Scripts.Systems
{
    public class ReputationSystem : IProtoInitSystem, IProtoRunSystem
    {
        private ProtoIt _it;
        [DI] private ProtoWorld _world;
        [DI] private BaseAspect _baseAspect;
        private readonly ReputationUIController _reputationUIController;
        private readonly LevelState _state;
        private readonly SoundManager _soundManager;
        private readonly GameResources _gameResources;

        public ReputationSystem(LevelState state, ReputationUIController reputationUIController, SoundManager soundManager, GameResources gameResources)
        {
            _reputationUIController = reputationUIController;
            _state = state;
            _soundManager = soundManager;
        }

        public void Init(IProtoSystems systems)
        {
            _it = new(new[] { typeof(ReputationRequest) });
            _it.Init(_world);
            
            _reputationUIController.Setup(_state.Reputation);
        }

        public void Run()
        {
            foreach (var reputationRequestEntity in _it)
            {
                ref var rr = ref reputationRequestEntity.Get<ReputationRequest>();
                _state.Reputation -= rr.Diff;

                _reputationUIController.UpdateRep(_state.Reputation);
                _world.NewEntityWith<PlaySFXRequest>().SoundType = SoundType.ReputationLoss;
                
                _baseAspect.ReputationRequestPool.Del(reputationRequestEntity);
            }
        }
    }
}