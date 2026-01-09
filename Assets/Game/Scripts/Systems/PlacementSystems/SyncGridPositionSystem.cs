using Leopotam.EcsProto;
using Leopotam.EcsProto.QoL;

public class SyncGridPositionSystem : IProtoInitSystem, IProtoRunSystem, IProtoDestroySystem
{
    [DI] readonly PlacementAspect _placementAspect;
    [DI] readonly PhysicsAspect _physicsAspect;
    [DI] readonly ProtoWorld _world;

    private readonly PlacementGrid _worldGrid;
    private ProtoIt _iteratorEvent;

    public SyncGridPositionSystem(PlacementGrid placementGrid) =>
        _worldGrid = placementGrid;

    public void Init(IProtoSystems systems)
    {
        _iteratorEvent = new ProtoIt(new[] { typeof(SyncGridPositionEvent), typeof(GridPositionComponent), typeof(PositionComponent) });
        _iteratorEvent.Init(_world);
    }

    public void Run()
    {
        foreach (var eventEntity in _iteratorEvent)
        {
            ref var syncEvent = ref _placementAspect.SyncGridPositionEventPool.Get(eventEntity);
            var worldPos = syncEvent.transform.position;
            
            var posInGrid = _worldGrid.WorldToGrid(worldPos);

            _worldGrid.AddElement(posInGrid);

            ref var gridPos = ref _physicsAspect.GridPositionPool.Get(eventEntity);
            gridPos.Position = posInGrid;

            ref var pos = ref _physicsAspect.PositionPool.Get(eventEntity);
            pos.Position = worldPos;

            _placementAspect.SyncGridPositionEventPool.DelIfExists(eventEntity);
        }
    }

    public void Destroy() => 
        _iteratorEvent = null;
}