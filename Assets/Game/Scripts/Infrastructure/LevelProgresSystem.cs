using Game.Scripts.UIControllers;
using Leopotam.EcsProto;

namespace Game.Scripts.Infrastructure
{
    public class LevelProgresSystem : IProtoRunSystem
    {
        private readonly LevelStateService  _levelStateService;
        private readonly LevelProgressUIController  _levelProgressUIController;

        private LevelProgresSystem(LevelStateService levelStateService, LevelProgressUIController levelProgressUIController)
        {
            _levelStateService = levelStateService;
            _levelProgressUIController = levelProgressUIController;
        }

        public void Run()
        {
            var progress = _levelStateService.ElapsedTime / _levelStateService.LevelDuration;
            _levelProgressUIController.UpdateState(progress);
        }
    }
}