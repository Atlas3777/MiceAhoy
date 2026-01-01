using Game.Script.Systems;
using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using VContainer;

namespace Game.Scripts.TutorialTasks
{
    namespace Game.Scripts.TutorialTasks
    {
        [Serializable]
        public sealed class PickStep : TutorialStep
        {
            public override string Description => "Подойдите к столу и возьмите предмет [E]";

            public override async UniTask Execute(IObjectResolver resolver, CancellationToken ct)
            {
                var pickPlaceSystem = resolver.Resolve<PickPlaceSystem>();
                var tutorialUIController = resolver.Resolve<TutorialUIController>();

                var tcsPicked = new UniTaskCompletionSource();

                pickPlaceSystem.PlayerPick += OnPicked;

                try
                {
                    await UniTask.WhenAll(
                        tutorialUIController.ShowTaskAsync(Description, ct),
                        tcsPicked.Task.AttachExternalCancellation(ct)
                    );

                    await tutorialUIController.ShowCompletedAsync(ct);
                }
                finally
                {
                    pickPlaceSystem.PlayerPick -= OnPicked;
                }

                return;

                void OnPicked() => tcsPicked.TrySetResult();
            }
        }
    }
}