// using System.Collections.Generic;
// using Game.Script.Aspects;
// using Game.Scripts.Aspects;
// using Leopotam.EcsProto;
// using Leopotam.EcsProto.QoL;
// using UnityEngine;
//
// namespace Game.Script.Systems
// {
//     public class GroupArrivingRegistrySystem : IProtoInitSystem, IProtoRunSystem
//     {
//         [DI] private GuestGroupAspect _guestGroupAspect;
//         [DI] private GuestAspect _guestAspect;
//         
//         [DI] private ProtoWorld _world;
//         private ProtoItExc _walkingGroupIterator;
//         
//         public void Init(IProtoSystems systems)
//         {
//             _walkingGroupIterator = new(new[]
//             {
//                 typeof(GuestGroupTag), typeof(GroupIsWalkingTag),
//             }, new[] { typeof(TimerComponent), typeof(WaitingTakeOrderTag), typeof(WaitingOrderTag),
//                 typeof(GuestGroupServedTag) });
//             _walkingGroupIterator.Init(_world);
//         }
//
//         public void Run()
//         {
//             foreach (var groupEntity in _walkingGroupIterator)
//             {
//                 var packedGuests = _guestGroupAspect.GuestGroupPool
//                     .Get(groupEntity).includedGuests;
//                 var isEveryGuestArrived = true;
//                 
//                 foreach (var packedGuest in packedGuests)
//                 {
//                     if (!packedGuest.TryUnpack(out _, out var guest))
//                     {
//                         Debug.LogWarning($"Failed to unpack {packedGuest}");
//                         continue;
//                     }
//
//                     if (_guestAspect.ReachedTargetPositionEventPool.Has(guest))
//                     {
//                         Debug.LogWarning("ало мы тут да");
//                         _guestAspect.GuestDidArriveTagPool.Add(guest);
//                     }
//
//                     if (!_guestAspect.GuestDidArriveTagPool.Has(guest))
//                     {
//                         isEveryGuestArrived = false;
//                     }
//                 }
//
//                 if (isEveryGuestArrived)
//                 {
//                     Debug.Log("все здесь, начинаем");
//                     _guestGroupAspect.GroupArrivedEventPool.Add(groupEntity);
//                 }
//             }
//         }
//     }
// }