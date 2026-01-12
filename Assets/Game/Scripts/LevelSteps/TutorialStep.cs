using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Game.Script.Systems;
using Game.Scripts.Aspects;
using Game.Scripts.Infrastructure;
using Game.Scripts.Input;
using Game.Scripts.Systems;
using Game.Scripts.UIControllers;
using Leopotam.EcsProto.QoL;
using Leopotam.EcsProto.Unity;
using VContainer;

namespace Game.Scripts.LevelSteps
{
    [Serializable]
    public sealed class TutorialStep : LevelStep
    {
        public override string Description => "TutorialStep";

        private SoundManager _soundManager;
        private GameResources _resources;
        private TutorialUIController _ui;

        public override async UniTask Execute(IObjectResolver resolver, CancellationToken ct)
        {
            // Инициализация зависимостей
            var joinListener = resolver.Resolve<JoinListener>();
            _soundManager = resolver.Resolve<SoundManager>();
            _resources = resolver.Resolve<GameResources>();
            _ui = resolver.Resolve<TutorialUIController>();
            resolver.Resolve<LevelProgressUIController>().GetConteiner().alpha = 0;
            resolver.Resolve<LevelDisplayUI>().Show("Обучение");

            joinListener.Enable();

            // Музыка на фоне
            _soundManager.PlayMusicAsync(_resources.SoundsLink.tutorial).Forget();

            // Последовательность шагов
            await MoveTask(resolver, ct);
            await PickTask(resolver, ct);
            await CookMeatTask(resolver, ct);
            await FirstCookedTask(resolver, ct);
            await BurnedTask(resolver, ct);
            await DeleteBurnedEatTask(resolver, ct);
            await GuestSpawnTask(resolver, ct);
            await GuestFedTask(resolver, ct);
            await GuestAngerTask(resolver, ct);
            await CompleteTutorial(resolver, ct);
        }

        public override void Exit()
        {
        }

        #region Infrastructure

        /// <summary>
        /// Элегантная обертка для выполнения шага с текстом, ожиданием и звуком.
        /// </summary>
        private async UniTask ExecuteStep(string taskText, UniTask completionTask, CancellationToken ct)
        {
            await _ui.ShowTaskAsync(taskText, ct);

            // Ждем завершения логики задачи
            await completionTask.AttachExternalCancellation(ct);

            // Воспроизводим звук и показываем "Галочку"
            _soundManager.PlaySfx(_resources.SoundsLink.cooking_done);
            await _ui.ShowCompletedAsync(ct);
        }

        #endregion

        private async UniTask MoveTask(IObjectResolver resolver, CancellationToken ct)
        {
            var movementSystem = resolver.Resolve<PlayerMovementSystem>();
            var tcs = new UniTaskCompletionSource();
            Action action = () => tcs.TrySetResult();

            movementSystem.PlayerMoved += action;
            try
            {
                await ExecuteStep("Используй WASD, чтобы двигаться", tcs.Task, ct);
            }
            finally
            {
                movementSystem.PlayerMoved -= action;
            }
        }

        private async UniTask PickTask(IObjectResolver resolver, CancellationToken ct)
        {
            var pickPlaceSystem = resolver.Resolve<PickPlaceSystem>();
            var tcs = new UniTaskCompletionSource();
            Action action = () => tcs.TrySetResult();

            pickPlaceSystem.PlayerPick += action;
            try
            {
                await ExecuteStep("Подойдите к холодильнику и возьмите предмет [E]", tcs.Task, ct);
            }
            finally
            {
                pickPlaceSystem.PlayerPick -= action;
            }
        }

        private async UniTask CookMeatTask(IObjectResolver resolver, CancellationToken ct)
        {
            var stoveSystem = resolver.Resolve<StoveSystem>();
            var tcs = new UniTaskCompletionSource();
            Action action = () => tcs.TrySetResult();

            stoveSystem.OnItemPlacedToStove += action;
            try
            {
                await ExecuteStep("Положите мясо на плиту", tcs.Task, ct);
            }
            finally
            {
                stoveSystem.OnItemPlacedToStove -= action;
            }
        }

        private async UniTask FirstCookedTask(IObjectResolver resolver, CancellationToken ct)
        {
            var stoveSystem = resolver.Resolve<StoveSystem>();
            var tcs = new UniTaskCompletionSource();
            Action action = () => tcs.TrySetResult();

            stoveSystem.OnItemCooked += action;

            try
            {
                await _ui.ShowTaskAsync("Мясо готовится. Чем дольше — тем сытнее.", ct);

                await tcs.Task.AttachExternalCancellation(ct);

                _soundManager.PlaySfx(_resources.SoundsLink.cooking_done);
            }
            finally
            {
                stoveSystem.OnItemCooked -= action;
            }
        }

        private async UniTask BurnedTask(IObjectResolver resolver, CancellationToken ct)
        {
            var stoveSystem = resolver.Resolve<StoveSystem>();
            var tcs = new UniTaskCompletionSource();
            Action action = () => tcs.TrySetResult();

            stoveSystem.OnItemBurned += action;

            try
            {
                await _ui.ShowTaskAsync("Не переборщите! Сгоревшую еду нужно выкинуть.", ct);

                await tcs.Task.AttachExternalCancellation(ct);

                _soundManager.PlaySfx(_resources.SoundsLink.cooking_done);
                await _ui.ShowCompletedAsync(ct);
            }
            finally
            {
                stoveSystem.OnItemBurned -= action;
            }
        }

        private async UniTask DeleteBurnedEatTask(IObjectResolver resolver, CancellationToken ct)
        {
            var pickPlaceSystem = resolver.Resolve<PickPlaceSystem>();
            var tcs = new UniTaskCompletionSource();
            Action action = () => tcs.TrySetResult();

            pickPlaceSystem.DeletedBernedItem += action;
            try
            {
                await ExecuteStep("Выбросьте пережаренную еду в мусорку", tcs.Task, ct);
            }
            finally
            {
                pickPlaceSystem.DeletedBernedItem -= action;
            }
        }

        private async UniTask GuestFedTask(IObjectResolver resolver, CancellationToken ct)
        {
            var happyGuestSystem = resolver.Resolve<HappyGuestLeaveSystem>();
            var tcs = new UniTaskCompletionSource();
            Action action = () => tcs.TrySetResult();

            happyGuestSystem.OnGuestFed += action;
            try
            {
                await ExecuteStep("Накормите гостя", tcs.Task, ct);
            }
            finally
            {
                happyGuestSystem.OnGuestFed -= action;
            }
        }

        public async UniTask GuestSpawnTask(IObjectResolver resolver, CancellationToken ct)
        {
            var levelFlowController = resolver.Resolve<LevelFlowController>();

            // Шаг с паузой ECS требует ручного управления звуком
            await _ui.ShowTaskAsync("К вам будут приходить гости", ct);

            var world = ProtoUnityWorlds.Get();
            var guest = _resources.GuestProfilesLink.BaseGuestProfile;
            world.Aspect<BaseAspect>().SpawnGuestRequestPool.NewEntity().Profile = guest;

            await UniTask.Delay(1000, cancellationToken: ct); // Даем время осознать появление
            _soundManager.PlaySfx(_resources.SoundsLink.cooking_done);

            await _ui.ShowTaskAsync("Они очень хотят кушать", ct);
            await UniTask.Delay(1500, cancellationToken: ct);

            levelFlowController.SetCurrentLevelPhase(GameplayPhase.EcsPause);
            await _ui.ShowTaskAsync("Если вы не успеете их накормить - проиграете", ct);
            await UniTask.Delay(2000, cancellationToken: ct);

            levelFlowController.SetCurrentLevelPhase(GameplayPhase.None);
            await _ui.HideTaskAsync(ct);
        }

        private async UniTask GuestAngerTask(IObjectResolver resolver, CancellationToken ct)
        {
            var angrySystem = resolver.Resolve<AngryGuestLeaveSystem>();
            var tcs = new UniTaskCompletionSource();
            Action action = () => tcs.TrySetResult();

            await UniTask.Delay(100, cancellationToken: ct);

            angrySystem.AngryGuestLeave += action;

            try
            {
                var world = ProtoUnityWorlds.Get();
                var guestProfile = _resources.GuestProfilesLink.TutorialGuestProfile;
                world.Aspect<BaseAspect>().SpawnGuestRequestPool.NewEntity().Profile = guestProfile;

                await _ui.ShowTaskAsync("Некоторые гости очень очень голодны...", ct);

                await tcs.Task.AttachExternalCancellation(ct);

                _soundManager.PlaySfx(_resources.SoundsLink.cooking_done);

                await _ui.ShowTaskAsync("Если гость уйдет голодным — вы потеряете звезду. Всего их 3 на уровень!", ct);
                await UniTask.Delay(3000, cancellationToken: ct);
            }
            finally
            {
                angrySystem.AngryGuestLeave -= action;
            }
        }

        public async UniTask CompleteTutorial(IObjectResolver resolver, CancellationToken ct)
        {
            var sc = resolver.Resolve<SceneController>();
            _soundManager.PlaySfx(_resources.SoundsLink.cooking_done);
            await _ui.ShowAllCompletedAsync(ct);
            await UniTask.Delay(1000, cancellationToken: ct);

            await sc.LoadMainMenuSceneAsync();
        }
    }
}