using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using VContainer;

namespace Game.Scripts.TutorialTasks
{
    [Serializable]
    public sealed class MoveStep : TutorialStep
    {
        public override string Description => "Используй WASD, чтобы двигаться";

        public override async UniTask Execute(IObjectResolver resolver, CancellationToken ct)
        {
            var movementSystem = resolver.Resolve<PlayerMovementSystem>();
            var tutorialUIController = resolver.Resolve<TutorialUIController>();

            var tcsMoved = new UniTaskCompletionSource();

            movementSystem.PlayerMoved += OnMoved;

            try
            {
                await UniTask.WhenAll(
                    tutorialUIController.ShowTaskAsync(Description, ct),
                    tcsMoved.Task.AttachExternalCancellation(ct)
                );

                await tutorialUIController.ShowCompletedAsync(ct);
            }
            finally
            {
                movementSystem.PlayerMoved -= OnMoved;
            }

            return;

            void OnMoved() => tcsMoved.TrySetResult();
        }
    }
}