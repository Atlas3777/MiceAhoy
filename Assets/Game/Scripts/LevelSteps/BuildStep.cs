using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Game.Scripts;
using Game.Scripts.Infrastructure;
using Game.Scripts.Input;
using Game.Scripts.LevelSteps;
using Game.Scripts.Systems;
using TMPro;
using VContainer;

[Serializable]
public class BuildStep : LevelStep
{
    public override string Description => "Строительство";
    public override GameplayPhase? Phase => GameplayPhase.Build;

    public override async UniTask Execute(IObjectResolver resolver, CancellationToken ct)
    {
        var startDaySystem = resolver.Resolve<StartDaySystem>();
        var joinListener = resolver.Resolve<JoinListener>();
        var buildModeService = resolver.Resolve<BuildModeService>();
        
        
        var tcs = new UniTaskCompletionSource();
        var soundManager = resolver.Resolve<SoundManager>();
        var gameResources = resolver.Resolve<GameResources>();
        
        var text = resolver.Resolve<TMP_Text>("status");
        text.text = "Статус: Строительство \n \nНажмите [E], чтобы поднять и поставить объект \n \nЧтобы начать день откройте входную дверь [E]";

        joinListener.Enable();
        buildModeService.EnterBuildMode();
        soundManager.PlayMusicAsync(gameResources.SoundsLink.bild).Forget();

        
        Action onStartAction = null;
        onStartAction = () =>
        {
            startDaySystem.OnStart -= onStartAction;
            tcs.TrySetResult(); 
        };

        startDaySystem.OnStart += onStartAction;

        try
        {
            await tcs.Task.AttachExternalCancellation(ct);
        }
        finally
        {
            startDaySystem.OnStart -= onStartAction;
            joinListener.Disable();
            buildModeService.ExitBuildMode();
        }
    }
}