using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Game.Scripts.DISystem;
using Game.Scripts.Infrastructure;
using Game.Scripts.UIControllers;
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

            // Данные награды (можно брать из конфига уровня или ресурсов)
            var rewardSprite = gameResources.box; 
            var rewardName = "Новый 2D Холодильник!";

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
            
            // Логика перехода на следующий уровень
            // Можно вызвать метод у SceneController для загрузки следующего индекса

            await sceneController.LoadMainGameSceneAsync();
        }
    }
}