using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Game.Script.Aspects;
using Game.Script.Systems;
using Leopotam.EcsProto.QoL;
using Leopotam.EcsProto.Unity;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using VContainer;
using Object = UnityEngine.Object;

namespace Game.Scripts.TutorialTasks
{
    [Serializable]
    public sealed class GuestStep : TutorialStep
    {
        public override string Description => "Накормите";

        public override async UniTask Execute(IObjectResolver resolver, CancellationToken ct)
        {
            var gameResources = resolver.Resolve<GameResources>();
            var tableNotificationSystem = resolver.Resolve<TableNotificationSystem>();
            var tutorialUIController = resolver.Resolve<TutorialUIController>();
            
            var r = gameResources.GuestSpawner;
            
            var g = Object.Instantiate(r.gameObject);
            Object.Destroy(g);

            var tcsMoved = new UniTaskCompletionSource();

            tableNotificationSystem.GuestGroupServedTagPool += OnServed;

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
                tableNotificationSystem.GuestGroupServedTagPool -= OnServed;
            }

            return;

            void OnServed() => tcsMoved.TrySetResult();
        }
    }
}