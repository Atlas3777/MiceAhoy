using Game.Scripts.UIControllers;
using Leopotam.EcsProto;

namespace Game.Scripts.Infrastructure
{
    public class LevelProgresSystem : IProtoRunSystem
    {
        private readonly LevelState  _levelState;
        private readonly LevelProgressUIController  _levelProgressUIController;

        private LevelProgresSystem(LevelState levelState, LevelProgressUIController levelProgressUIController)
        {
            _levelState = levelState;
            _levelProgressUIController = levelProgressUIController;
        }

        public void Run()
        {
            var progress = _levelState.ElapsedTime / _levelState.LevelDuration;
            _levelProgressUIController.UpdateState(progress);
        }
    }
}