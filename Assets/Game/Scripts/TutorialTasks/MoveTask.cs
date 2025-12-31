using System;
using VContainer;

namespace Game.Scripts.TutorialTasks
{
    [Serializable]
    public class MoveTask : TutorialTask
    {
        public override string Description => "TEST-TEST Используй WASD, чтобы двигаться";
    
        private PlayerMovementSystem _movementSystem;

        public override void Enter(IObjectResolver resolver)
        {
            if (resolver.TryResolve<PlayerMovementSystem>(out _movementSystem))
                _movementSystem.PlayerMoved += HandleMovement;
            else
                UnityEngine.Debug.LogError("MoveTask: PlayerMovementSystem не найден в контейнере!");
        }

        private void HandleMovement()
        {
            NotifyComplete();
        }

        public override void Exit()
        {
            if (_movementSystem != null)
            {
                _movementSystem.PlayerMoved -= HandleMovement;
            }
        }
    }
}