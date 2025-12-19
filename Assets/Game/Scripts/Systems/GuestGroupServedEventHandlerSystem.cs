// using Game.Script.Aspects;
// using Leopotam.EcsProto;
// using Leopotam.EcsProto.QoL;
//
// namespace Game.Script.Systems
// {
//     public class GuestGroupServedEventHandlerSystem : IProtoInitSystem, IProtoRunSystem
//     {
//         [DI] private GuestGroupAspect _guestGroupAspect;
//         [DI] private GuestAspect _guestAspect;
//         [DI] private BaseAspect _baseAspect;
//         
//         private ProtoIt _servedGroupsIterator;
//
//         public void Init(IProtoSystems systems)
//         {
//             var world = systems.World();
//             _servedGroupsIterator = new ProtoIt(new[] { typeof(GuestGroupServedEvent) });
//             _servedGroupsIterator.Init(world);
//         }
//
//         public void Run()
//         {
//             foreach (var guestGroupEntity in _servedGroupsIterator)
//             {
//                 // Добавляем тег, что группа обслужена
//                 _guestGroupAspect.GuestGroupServedTagPool.Add(guestGroupEntity);
//                 _baseAspect.TimerCompletedPool.Add(guestGroupEntity);
//
//                 // Получаем список гостей группы
//                 ref var guestGroup = ref _guestGroupAspect.GuestGroupPool.Get(guestGroupEntity);
//                 foreach (var packedGuest in guestGroup.includedGuests)
//                 {
//                     if (packedGuest.TryUnpack(out _, out var guestEntity))
//                     {
//                         // Добавляем тег, что гость уходит
//                         _guestAspect.GuestIsWalkingTagPool.Add(guestEntity);
//                     }
//                 }
//
//                 // Удаляем событие после обработки
//                 _guestGroupAspect.GuestGroupServedEventPool.Del(guestGroupEntity);
//             }
//         }
//     }
// }