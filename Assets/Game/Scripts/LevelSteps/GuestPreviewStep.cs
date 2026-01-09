using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using Game.Scripts.DISystem;
using Game.Scripts.Infrastructure;
using PrimeTween;
using Unity.Cinemachine;
using UnityEngine;
using VContainer;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace Game.Scripts.LevelSteps
{
    [Serializable]
    public class GuestPreviewStep : LevelStep
    {
        public override string Description => "Пролет камеры (Intro) к врагам";
        public override GameplayPhase? Phase => GameplayPhase.EcsPause;

        [SerializeField] private int _previewCount = 5;
        private readonly List<GameObject> _spawnedPreviews = new();
        private Vector3 _cameraOffset = new(0, 0, -5f);
        private int _spawnSpread = 1;
        private BoxCollider _spawnZone;
        private float _minDistanceBetweenGuests;

        public override async UniTask Execute(IObjectResolver resolver, CancellationToken ct)
        {
            var introCam = resolver.Resolve<CinemachineCamera>(GameCameraType.Intro);
            var gameplayCam = resolver.Resolve<CinemachineCamera>(GameCameraType.Gameplay);
            var config = resolver.Resolve<LevelConfig>();
            _spawnZone = resolver.Resolve<LevelContext>().enemyBox;

            Vector3 startPos = gameplayCam.transform.position;

            Vector3 targetPos = startPos + _cameraOffset;

            introCam.transform.position = startPos;
            introCam.Priority = 20;

            SpawnPreviewGuests(config);

            await Tween.Position(introCam.transform, targetPos, 2f, Ease.OutCubic).ToUniTask(cancellationToken: ct);

            // Задержка на просмотр
            await UniTask.Delay(1500, cancellationToken: ct);

            // 5. Анимация полета "Назад"
            await Tween.Position(introCam.transform, startPos, 2f, Ease.InCubic).ToUniTask(cancellationToken: ct);

            CleanupPreviews();

            introCam.Priority = 9;
            await UniTask.Delay(1000, cancellationToken: ct);
        }


        private GuestProfile GetRandomProfile(List<GuestProfile> profiles)
        {
            float totalWeight = 0;
            foreach (var p in profiles) totalWeight += p.Weight;

            float randomValue = Random.Range(0, totalWeight);
            float currentSum = 0;

            foreach (var p in profiles)
            {
                currentSum += p.Weight;
                if (randomValue <= currentSum) return p;
            }

            return profiles[0];
        }

        private void CleanupPreviews()
        {
            foreach (var guest in _spawnedPreviews)
            {
                if (guest != null) Object.Destroy(guest);
            }

            _spawnedPreviews.Clear();
        }

        private void SpawnPreviewGuests(LevelConfig config)
        {
            if (config.AvailableGuests == null || config.AvailableGuests.Count == 0) return;

            Bounds bounds = _spawnZone.bounds;
            List<Vector3> spawnPoints = GenerateSpawnPoints(bounds, _previewCount);

            for (int i = 0; i < spawnPoints.Count; i++)
            {
                var profile = GetRandomProfile(config.AvailableGuests);
                if (profile?.Guest == null) continue;

                // Инстанцируем в рассчитанную точку
                var instance = Object.Instantiate(profile.Guest, spawnPoints[i], Quaternion.identity);
                _spawnedPreviews.Add(instance);
            }
        }


        private List<Vector3> GenerateSpawnPoints(Bounds bounds, int count)
        {
            List<Vector3> points = new List<Vector3>();

            // Вычисляем, сколько "клеток" у нас есть по горизонтали (X)
            // Так как вы просили "полоску", приоритет отдаем оси X
            float width = bounds.size.x;
            float depth = bounds.size.z;

            // Попытка простого распределения по сетке внутри Bounds
            for (int i = 0; i < count; i++)
            {
                for (int attempt = 0; attempt < 10; attempt++) // 10 попыток найти место
                {
                    Vector3 candidate = new Vector3(
                        Random.Range(bounds.min.x, bounds.max.x),
                        bounds.center.y, // Базовая высота зоны
                        Random.Range(bounds.min.z, bounds.max.z)
                    );

                    // Проверка на дистанцию с уже созданными точками
                    bool farEnough = true;
                    foreach (var p in points)
                    {
                        if (Vector3.Distance(candidate, p) < _minDistanceBetweenGuests)
                        {
                            farEnough = false;
                            break;
                        }
                    }

                    if (farEnough)
                    {
                        points.Add(candidate);
                        break;
                    }
                }
            }

            return points;
        }
    }
}