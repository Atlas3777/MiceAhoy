// using Game.Script.Aspects;
// using Leopotam.EcsProto;
// using Leopotam.EcsProto.QoL;
// using Leopotam.EcsProto.Unity;
// using UnityEngine;
// using UnityEngine.Playables;
// using VContainer;
//
// public class LevelReciever : MonoBehaviour, INotificationReceiver
// {
//     private GuestAspect _guestAspect;
//     private ProtoWorld _world;
//     private GameResources _gameResources;
//     private ProtoPackedEntityWithWorld _entity;
//     
//     [Inject]
//     private void Setup(IObjectResolver container)
//     {
//         _gameResources = container.Resolve<GameResources>();
//     }
//     
//     public void Start()
//     {
//         _world = ProtoUnityWorlds.Get();
//         _guestAspect = (GuestAspect)_world.Aspect(typeof(GuestAspect));
//         var spawner = _gameResources.GuestSpawner;
//         var goSpawner = Instantiate(spawner);
//         var auth = goSpawner.GetComponent<CustomAuthoring>();
//         
//         auth.ProcessAuthoring();
//         _entity = auth.Entity();
//     }
//
//     public void OnNotify(Playable origin, INotification notification, object context)
//     {
//         if (notification is GuestGroupSpawnEventMarker spawnMarker)
//         {
//             SpawnGuestGroupEntity();
//         }
//     }
//
//     private void SpawnGuestGroupEntity()
//     {
//         _entity.TryUnpack(out _, out var protoEntity);
//         {
//             _guestAspect.GuestRequestEventPool.Add(protoEntity);
//         }
//     }
// }