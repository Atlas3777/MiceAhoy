using Game.Script.Aspects;
using Leopotam.EcsProto;
using Leopotam.EcsProto.QoL;
using Leopotam.EcsProto.Unity;
using UnityEngine;
using UnityEngine.Playables;
using VContainer;

public class LevelReciever : MonoBehaviour, INotificationReceiver
{
    private GuestGroupAspect _guestGroupAspect;
    private ProtoWorld _world;
    private GameResources _gameResources;
    
    [Inject]
    private void Setup(IObjectResolver container)
    {
        _gameResources = container.Resolve<GameResources>();
    }
    
    public void Start()
    {
        _world = ProtoUnityWorlds.Get();
        _guestGroupAspect = (GuestGroupAspect)_world.Aspect(typeof(GuestGroupAspect));
    }

    public void OnNotify(Playable origin, INotification notification, object context)
    {
        if (notification is GuestGroupSpawnEventMarker spawnMarker)
        {
            SpawnGuestGroupEntity(spawnMarker);
        }
    }

    private void SpawnGuestGroupEntity(GuestGroupSpawnEventMarker spawnMarker)
    {
        var group = _gameResources.GuestGroup.gameObject;
        var goGroup = Instantiate(group);
        var auth = goGroup.GetComponent<CustomAuthoring>();
        
        auth.ProcessAuthoring();
        var entity = auth.Entity();
        
        entity.TryUnpack(out _, out var protoEntity);
        {
            ref var targetGroupSize = ref _guestGroupAspect.TargetGroupSizePool.Get(protoEntity);
            targetGroupSize.size = spawnMarker.count;
        }
    }
}