using Game.Script.Aspects;
using Leopotam.EcsProto;
using Leopotam.EcsProto.QoL;

namespace Game.Scripts.Systems
{
    public class HandleQueueLeavingSystem : IProtoInitSystem, IProtoRunSystem
    {
        [DI] private GuestAspect _guestAspect;
        [DI] private ProtoWorld _world;
        private ProtoIt _leavingGuestsIt;
        private ProtoIt _freeTableIt;
        
        public void Init(IProtoSystems systems)
        {
            _leavingGuestsIt = new(new[] { typeof(UpdateQueueEvent) });
            _freeTableIt = new(new[] { typeof(GuestTableIsFreeTag) });
            _leavingGuestsIt.Init(_world);
            _freeTableIt.Init(_world);
        }

        public void Run()
        {
            foreach (var guestEntity in _leavingGuestsIt)
            {
                foreach (var tableEntity in _freeTableIt)
                {
                    
                }
            }
        }
    }
}