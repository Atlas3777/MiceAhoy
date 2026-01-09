// using System.Linq;
// using Game.Script.Aspects;
// using Game.Scripts.Aspects;
// using Leopotam.EcsProto;
// using Leopotam.EcsProto.QoL;
// using UnityEngine;
//
// namespace Game.Scripts.Systems
// {
//     public class QueueWaitingVisualizationSystem : IProtoInitSystem, IProtoRunSystem
//     {
//         [DI] private GuestAspect _guestAspect;
//         [DI] private BaseAspect _baseAspect;
//         [DI] private ProtoWorld _world;
//
//         private ProtoIt _queueWaitingIt;
//         private ProtoIt _queueTimeoutIt;
//         
//         public void Init(IProtoSystems systems)
//         {
//             _queueWaitingIt = new (new[] { typeof(QueueComponent), typeof(QueueIsWaitingTag) });
//             _queueTimeoutIt = new ProtoIt(new[] { typeof(QueueComponent), typeof(UpdateQueueVisualEvent) });
//             _queueWaitingIt.Init(_world);
//             _queueTimeoutIt.Init(_world);
//         }
//
//         public void Run()
//         {
//             foreach (var queueEntity in _queueTimeoutIt)
//             {
//                 ref var queue = ref _guestAspect.QueueComponentPool.Get(queueEntity).Queue;
//                 if (queue.Count == 0)
//                 {
//                     _baseAspect.TimerCompletedPool.Add(queueEntity); // Просто чтобы скрыть таймер
//                     _guestAspect.QueueIsWaitingTagPool.Del(queueEntity);
//                 }
//             }
//             
//             foreach (var queueEntity in _queueWaitingIt)
//             {
//                 ref var queue = ref _guestAspect.QueueComponentPool.Get(queueEntity).Queue;
//                 if (queue.Count == 0)
//                 {
//                     Debug.LogWarning("Очередь здесь пустой быть не должна...");
//                     continue;
//                 }
//
//                 if (!_baseAspect.TimerPool.Has(queueEntity))
//                 {
//                     ref var timer = ref _baseAspect.TimerPool.Add(queueEntity);
//                     timer.Elapsed = 0f;
//                     timer.Completed = false;
//                     if (!queue.Peek().TryUnpack(out _, out var guest))
//                     {
//                         Debug.LogWarning("Первый умер");
//                         continue;
//                     }
//                     ref var guestState = ref _guestAspect.GuestStateComponentPool.GetOrAdd(guest);
//                     //timer.Duration = guestState.WaitingSeconds;
//                     timer.Duration = 5; // Debug
//                 }
//             }
//         }
//     }
// }