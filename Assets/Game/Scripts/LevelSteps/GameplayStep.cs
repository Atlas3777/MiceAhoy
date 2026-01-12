using System;
using System.Threading;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using Game.Scripts;
using Game.Scripts.Infrastructure;
using Game.Scripts.LevelSteps;
using Game.Scripts.Systems;
using Game.Scripts.UIControllers;
using TMPro;
using UnityEngine;
using VContainer;


[Serializable]
public class GameplayStep : LevelStep
{
    public override string Description => "Геймплей";
    public override GameplayPhase? Phase => GameplayPhase.Gameplay;

    public override async UniTask Execute(IObjectResolver resolver, CancellationToken ct)
    {
        var levelFlowController = resolver.Resolve<LevelFlowController>();
        var winLoseSystem = resolver.Resolve<WinLoseSystem>();
        var soundManager = resolver.Resolve<SoundManager>();
        var gameResources = resolver.Resolve<GameResources>();

        var resultSource = new UniTaskCompletionSource<GameResult>();
        
        var text = resolver.Resolve<TMP_Text>("status");
        text.text = "Статус: Обслуживание";

        soundManager.PlayMusicAsync(gameResources.SoundsLink.background_music).Forget();

        Action<GameResult> onFinish = (result) => resultSource.TrySetResult(result);

        winLoseSystem.OnGameFinished += onFinish;

        GameResult result;
        try
        {
            result = await resultSource.Task.AttachExternalCancellation(ct);
        }
        finally
        {
            winLoseSystem.OnGameFinished -= onFinish;
        }

        levelFlowController.SetCurrentLevelPhase(GameplayPhase.EcsPause);

        soundManager.StopMusicAsync().Forget();
        
        if (result == GameResult.Lose)
        {
            await HandleLose(resolver, ct);
        }
        else
        {
            await HandleWin(resolver, ct);
        }

        Debug.Log("Gameplay finished with Win. Moving to next step.");
    }

    private async UniTask HandleWin(IObjectResolver resolver, CancellationToken ct)
    {
        var sceneManager = resolver.Resolve<SceneController>();
        var saveService = resolver.Resolve<SaveService>();

        if (saveService.Data.ContinueCompany)
        {
            saveService.Data.CompanyLevelIndex++;
            saveService.Data.CurrentLevelIndex++;
            await sceneManager.LoadMainGameSceneAsync();
        }
        else
        {
            await sceneManager.LoadMainMenuSceneAsync();
        }
    }

    private async UniTask HandleLose(IObjectResolver resolver, CancellationToken ct)
    {
        var view = resolver.Resolve<LoseUIController>();
        var sceneManager = resolver.Resolve<SceneController>();

        var choice = await view.WaitForChoice(ct);

        if (choice == LoseButtonResult.Retry)
        {
            await sceneManager.ReloadSceneAsync();
        }
        else
        {
            await sceneManager.LoadMainGameSceneAsync();
        }
    }
}