using System;
using System.Threading;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using Game.Script.Systems;
using Game.Scripts.Aspects;
using Leopotam.EcsProto.QoL;
using Leopotam.EcsProto.Unity;
using VContainer;

namespace Game.Scripts.LevelSteps
{
    [Serializable]
    public sealed class TutorialStep : LevelStep
    {
        public override string Description => "TutorialStep";

        public override async UniTask Execute(IObjectResolver resolver, CancellationToken ct)
        {
            // await MoveTask(resolver, ct);
            // await PickTask(resolver, ct);
            await GuestSpawnTask(resolver, ct);
            //await GuestFedTask(resolver, ct);
            await CompleteTutorial(resolver, ct);
        }

        private async UniTask GuestFedTask(IObjectResolver resolver, CancellationToken ct)
        {
            //var guestS = resolver.Resolve<GuestServicingFinalSystem>();

        }

        public override void Exit()
        {
        }

        public async UniTask GuestSpawnTask(IObjectResolver resolver, CancellationToken ct)
        {
            var gameResources = resolver.Resolve<GameResources>();
            var tutorialUIController = resolver.Resolve<TutorialUIController>();
            var levelFlowController = resolver.Resolve<LevelFlowController>();
            
            await tutorialUIController.ShowTaskAsync("К вам будут приходить гости", ct);
            
            var world = ProtoUnityWorlds.Get();
            var guest = gameResources.GuestProfilesLink.BaseGuestProfile;
            world.Aspect<BaseAspect>().SpawnGuestRequestPool.NewEntity().Profile = guest;


            await tutorialUIController.ShowTaskAsync("Они кушать хотят", ct);
            await tutorialUIController.HideTaskAsync(ct);

            levelFlowController.SetCurrentLevelPhase(GameplayPhase.EcsPause);
            
            await tutorialUIController.ShowTaskAsync("Не успел - проебал", ct);
            await tutorialUIController.HideTaskAsync(ct);

            levelFlowController.SetCurrentLevelPhase(GameplayPhase.None);
        }


        public async UniTask MoveTask(IObjectResolver resolver, CancellationToken ct)
        {
            var movementSystem = resolver.Resolve<PlayerMovementSystem>();
            var tutorialUIController = resolver.Resolve<TutorialUIController>();

            var tcsMoved = new UniTaskCompletionSource();

            movementSystem.PlayerMoved += OnMoved;

            try
            {
                await UniTask.WhenAll(
                    tutorialUIController.ShowTaskAsync("Используй WASD, чтобы двигаться", ct),
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

        public async UniTask PickTask(IObjectResolver resolver, CancellationToken ct)
        {
            var pickPlaceSystem = resolver.Resolve<PickPlaceSystem>();
            var tutorialUIController = resolver.Resolve<TutorialUIController>();

            var text = "Подойдите к холодильнику и возьмите предмет [E]";

            var tcsPicked = new UniTaskCompletionSource();

            pickPlaceSystem.PlayerPick += OnPicked;

            try
            {
                await UniTask.WhenAll(
                    tutorialUIController.ShowTaskAsync(text, ct),
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

        public async UniTask CompleteTutorial(IObjectResolver resolver, CancellationToken ct)
        {
            var tutorialUIController = resolver.Resolve<TutorialUIController>();
            await tutorialUIController.ShowAllCompletedAsync(ct);
        }
    }
}