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
            
            CreateGuests(gameResources.Guest.gameObject);
            // var r = gameResources.GuestSpawner;
            // var g = Object.Instantiate(r.gameObject);
            // var authoring = g.GetComponent<CustomAuthoring>();
            // authoring.ProcessAuthoring();
            // Object.Destroy(g);

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
    
        private void CreateGuests(GameObject _guestPrefab)
        {
            var world = ProtoUnityWorlds.Get();
            var guestAspect = (GuestAspect)world.Aspect(typeof(GuestAspect));
            var go = Object.Instantiate(_guestPrefab);
            var authoring = go.GetComponent<CustomAuthoring>();

            //authoring.ProcessAuthoring();
            var entity = authoring.Entity();
            Debug.Log(entity);
            if (!entity.TryUnpack(out _, out var unpackedEntity))
                Debug.Log("мать ебал автора ecs");
            
            if (guestAspect == null)
                Debug.Log("мать ебал автора ecs 2");
            
            // ref var goRef = ref guestAspect.GuestGameObjectRefComponentPool.Add(unpackedEntity);
            // goRef.GameObject = go;
            
            var agent = go.GetComponent<NavMeshAgent>();
            // Debug.Log("мать ебал автора ecs 3");
            // ref var agentComponent = ref guestAspect.NavMeshAgentComponentPool.Add(unpackedEntity);
            // agentComponent.Agent = agent;
            agent.updateRotation = false;
            agent.updateUpAxis = false;
        }
    }
}