using Game.Scripts.Input;
using Game.Scripts.LevelSteps;
using Unity.Cinemachine;
using VContainer.Unity;

namespace Game.Scripts.Infrastructure
{
    public class LevelBootstrap : IInitializable
    {
        private readonly PlayerSpawner _playerSpawner;
        private readonly LevelFlowController _levelFlowController;
        private readonly LevelRuntimeController _levelRuntimeController;
        private readonly CinemachineTargetGroup _targetGroup;
        private readonly LevelConfig _levelConfig;
        private readonly LevelContext _context;

        public LevelBootstrap(LevelFlowController levelFlowController, PlayerSpawner playerSpawner,
            LevelRuntimeController levelRuntimeController, CinemachineTargetGroup targetGroup, LevelConfig levelConfig,
            LevelContext context)
        {
            _levelFlowController = levelFlowController;
            _playerSpawner = playerSpawner;
            _levelRuntimeController = levelRuntimeController;
            _targetGroup = targetGroup;
            _levelConfig = levelConfig;
            _context = context;
        }

        public void Initialize()
        {
            _targetGroup.AddMember(_context.levelCenter, 4, 4);


            _levelFlowController.Start(_levelConfig.LevelStates);
            _playerSpawner.SetSpawnPoint(_context.positionsRegistry.PlayerSpawn);
            _levelRuntimeController.Start();
        }
    }
}