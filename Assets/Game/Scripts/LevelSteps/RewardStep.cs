using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Game.Scripts.DISystem;
using Game.Scripts.Infrastructure;
using Game.Scripts.UIControllers;
using TMPro;
using UnityEngine;
using VContainer;

namespace Game.Scripts.LevelSteps
{
    [Serializable]
    public class RewardStep : LevelStep
    {
        public override string Description => "Получение награды";
        
        public override GameplayPhase? Phase => GameplayPhase.EcsPause;

        public override async UniTask Execute(IObjectResolver resolver, CancellationToken ct)
        {
            var saveService = resolver.Resolve<SaveService>();
            var rewardUI = resolver.Resolve<RewardUIController>();
            var gameResources = resolver.Resolve<GameResources>();
            var sceneController = resolver.Resolve<SceneController>();
            
            var text = resolver.Resolve<TMP_Text>("status");
            text.text = "";

            // Данные награды (можно брать из конфига уровня или ресурсов)
            var rewardSprite = gameResources.stove_top; 
            var rewardName = "Получите дополнительную печь!";

            Debug.Log("Показываем награду...");
            saveService.Data.LevelIndex++;
            saveService.Save();
            
            try 
            {
                await rewardUI.ShowRewardAsync(rewardSprite, rewardName, ct);
            }
            catch (OperationCanceledException)
            {
                return;
            }

            Debug.Log("Награда принята. Переходим на следующий уровень.");
            

            await sceneController.LoadMainGameSceneAsync();
        }
    }
}