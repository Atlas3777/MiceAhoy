using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Game.Script.DISystem;
using PrimeTween;
using Unity.Cinemachine;
using UnityEngine;
using VContainer;

namespace Game.Scripts.LevelSteps
{
    [Serializable]
    public class ShowPreviewGuestStep : LevelStep
    {
        public override string Description => "Пролет камеры (Intro)";

        // Настройки пролета (можно вынести в конфиг)
        private readonly Vector3 _startPoint = new Vector3(0, 44.4f, -44.5f); // Центр
        private readonly Vector3 _leftPoint = new Vector3(-4.5f, 44.4f, -44.5f); // Лево

        public override async UniTask Execute(IObjectResolver resolver, CancellationToken ct)
        {
            var introCam = resolver.Resolve<CinemachineCamera>(GameCameraType.Intro);
            var gameplayCam = resolver.Resolve<CinemachineCamera>(GameCameraType.Gameplay);

            introCam.transform.position = _startPoint;
            introCam.Priority = 20;
            gameplayCam.Priority = 10;
            
            //await UniTask.Yield(PlayerLoopTiming.Update, ct);

            // Фаза 1: Летим влево
            await Tween.Position(introCam.transform, _leftPoint, 2f, Ease.InOutQuad).ToUniTask(cancellationToken: ct);

            await UniTask.Delay(1500, cancellationToken: ct);

            // Фаза 2: Летим обратно
            await Tween.Position(introCam.transform, _startPoint, 2f, Ease.InOutQuad).ToUniTask(cancellationToken: ct);

            introCam.Priority = 9; 
            
            // Ждем окончания бленда Cinemachine
            await UniTask.Delay(1000, cancellationToken: ct); 
        }
    }
}