using Cysharp.Threading.Tasks;
using Game.Scripts.Input;
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
        private readonly SoundManager _soundManager;

        public LevelBootstrap(LevelFlowController levelFlowController, PlayerSpawner playerSpawner,
            LevelRuntimeController levelRuntimeController, CinemachineTargetGroup targetGroup, LevelConfig levelConfig,
            LevelContext context, LevelDisplayUI levelDisplayUI, SaveService saveService,
            PlayerSessionService sessionService, SoundManager soundManager)
        {
            _levelFlowController = levelFlowController;
            _playerSpawner = playerSpawner;
            _levelRuntimeController = levelRuntimeController;
            _targetGroup = targetGroup;
            _levelConfig = levelConfig;
            _context = context;
            _saveService = saveService;
            _levelDisplayUI = levelDisplayUI;
            _soundManager = soundManager;
        }

        public void Initialize()
        {
            _soundManager.StopMusicAsync(false).Forget();
            
            _context.navMeshSurface.BuildNavMesh();
            _targetGroup.AddMember(_context.levelCenter, 4, 4);

            _levelDisplayUI.Show(_saveService.Data.CurrentLevelIndex);

            _playerSpawner.SetSpawnPoint(_context.positionsRegistry.PlayerSpawn);


            _playerSpawner.SpawnExistingPlayers();

            _levelFlowController.Start(_levelConfig.LevelStates);
            _levelRuntimeController.Start();
        }
    }
}