using Game.Scripts.UIControllers;
using Leopotam.EcsProto;

namespace Game.Scripts.Infrastructure
{
    public class LevelProgresSystem : IProtoRunSystem
    {
        private readonly RuntimeLevelState  _runtimeLevelState;
        private readonly LevelProgressUIController  _levelProgressUIController;

        private LevelProgresSystem(RuntimeLevelState runtimeLevelState, LevelProgressUIController levelProgressUIController)
        {
            _runtimeLevelState = runtimeLevelState;
            _levelProgressUIController = levelProgressUIController;
        }

        public void Run()
        {
            var progress = _runtimeLevelState.ElapsedTime / _runtimeLevelState.LevelDuration;
            _levelProgressUIController.UpdateState(progress);
        }
    }
}