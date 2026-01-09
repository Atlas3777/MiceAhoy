using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Game.Scripts.Infrastructure;
using Game.Scripts.Input;
using Game.Scripts.LevelSteps;
using Game.Scripts.Systems;
using Game.Scripts.UIControllers;
using UnityEngine;
using VContainer;

[Serializable]
public class BuildStep : LevelStep
{
    public override string Description => "Строительство";
    public override GameplayPhase? Phase => GameplayPhase.Build;

    public override async UniTask Execute(IObjectResolver resolver, CancellationToken ct)
    {
        var levelFlowController = resolver.Resolve<LevelFlowController>();
        var winLoseSystem = resolver.Resolve<WinLoseSystem>();
        var rr = resolver.Resolve<JoinListener>();
        
        var resultSource = new UniTaskCompletionSource<GameResult>();
        rr.Enable();

        
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

        if (result == GameResult.Lose)
        {
            levelFlowController.SetCurrentLevelPhase(GameplayPhase.EcsPause);
            await HandleLose(resolver, ct); 
        }
        
        levelFlowController.SetCurrentLevelPhase(GameplayPhase.EcsPause);
        Debug.Log("Gameplay finished with Win. Moving to next step.");
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