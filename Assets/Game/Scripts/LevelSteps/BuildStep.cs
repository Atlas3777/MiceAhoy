using System;
using System.Threading;
using Cysharp.Threading.Tasks;
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
        var playerPressedPSystem = resolver.Resolve<PlayerPressedPSystem>();
        var rr = resolver.Resolve<JoinListener>();
        var tcs = new UniTaskCompletionSource();
        
        var text = resolver.Resolve<TMP_Text>("status");
        text.text = "Статус: Строительство(нажмите[E] чтобы поднять и поставить объект) \n Чтобы начать день откройте входную дверь [E]";
        
        playerPressedPSystem.StartPlacementMode();

        rr.Enable(playerPressedPSystem);
        

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
            rr.Disable();
            playerPressedPSystem.EndPlacementMode();
        }
    }
}