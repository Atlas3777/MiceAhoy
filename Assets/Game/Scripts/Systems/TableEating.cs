using Game.Script.Aspects;
using Game.Scripts.Aspects;
using Leopotam.EcsProto;
using Leopotam.EcsProto.QoL;

namespace Game.Scripts.Systems
{
    public class TableEating : IProtoInitSystem, IProtoRunSystem
    {
        [DI] readonly ProtoWorld _world;
        [DI] readonly BaseAspect _baseAspect;
        [DI] readonly WorkstationsAspect _workstationsAspect;
        [DI] readonly GuestAspect  _guestAspect;

        private ProtoIt _it;

        public void Init(IProtoSystems systems)
        {
            _it = new ProtoIt(new[]
            {
                typeof(GuestTableComponent), typeof(ItemPlaceEvent), typeof(HolderComponent),
                typeof(PositionComponent), typeof(InteractableComponent), 
            });
            _it.Init(_world);
        }

        public void Run()
        {
            foreach (var tableEntity in _it)
            {
                ref var holderComponent = ref _baseAspect.HolderPool.Get(tableEntity);
                ref var guest = ref _workstationsAspect.GuestTablePool.Get(tableEntity).Guest.Get<GuestStateComponent>();

                // guest.Hunger -= holderComponent.ItemEntity.Get<>();
                //
                // var gues = _guestAspect.GuestStateComponentPool.Get(tableEntity);
            }
        }
    }
}