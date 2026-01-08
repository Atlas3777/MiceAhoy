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
        private readonly LevelDisplayUI _levelDisplayUI;
        private readonly SaveService _saveService;
        public LevelBootstrap(LevelFlowController levelFlowController, PlayerSpawner playerSpawner,
            LevelRuntimeController levelRuntimeController, CinemachineTargetGroup targetGroup, LevelConfig levelConfig,
            LevelContext context, LevelDisplayUI levelDisplayUI, SaveService saveService)
        {
            _levelFlowController = levelFlowController;
            _playerSpawner = playerSpawner;
            _levelRuntimeController = levelRuntimeController;
            _targetGroup = targetGroup;
            _levelConfig = levelConfig;
            _context = context;
            _saveService = saveService;
            _levelDisplayUI = levelDisplayUI;
        }

        public void Initialize()
        {
            _context.navMeshSurface.BuildNavMesh();
            _targetGroup.AddMember(_context.levelCenter, 4, 4);
            
            _levelDisplayUI.Show(_saveService.Data.LevelIndex);


            _levelFlowController.Start(_levelConfig.LevelStates);
            _playerSpawner.SetSpawnPoint(_context.positionsRegistry.PlayerSpawn);
            _levelRuntimeController.Start();
        }
    }
}