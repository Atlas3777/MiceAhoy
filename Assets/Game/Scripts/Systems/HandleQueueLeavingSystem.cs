using Game.Script.Aspects;
using Game.Scripts.Aspects;
using Leopotam.EcsProto;
using Leopotam.EcsProto.QoL;

namespace Game.Scripts.Systems
{
    public class HandleQueueLeavingSystem : IProtoInitSystem, IProtoRunSystem
    {
        [DI] private GuestAspect _guestAspect;
        [DI] private ProtoWorld _world;
        
        private ProtoIt _leavingGuestsIt;
        private ProtoIt _updatingQueueIt;
        
        public void Init(IProtoSystems systems)
        {
            _leavingGuestsIt = new(new[] { typeof(GuestLeavingQueueEvent) });
            _updatingQueueIt = new(new[] { typeof(QueueComponent), typeof(QueueIsNotEmptyTag), typeof(UpdateQueueEvent) });
            _leavingGuestsIt.Init(_world);
            _updatingQueueIt.Init(_world);
        }

        public void Run()
        {
            foreach (var guestEntity in _leavingGuestsIt)
            {
                
            }
        }
    }
}