using Game.Script.Aspects;
using Game.Scripts.Aspects;
using Leopotam.EcsProto;
using Leopotam.EcsProto.QoL;
using UnityEngine;

namespace Game.Script.Systems
{
    public class GuestWaitingSystem : IProtoInitSystem, IProtoRunSystem, IProtoDestroySystem
    {
        [DI] readonly ProtoWorld _world;
        [DI] readonly PlayerAspect _playerAspect;
        [DI] readonly ViewAspect _viewAspect;
        [DI] readonly BaseAspect _baseAspect;
        [DI] readonly GuestAspect _guestAspect;
        [DI] readonly WorkstationsAspect _workstationsAspect;

        private ProtoItExc _startWaitingOrderIt;

        public void Init(IProtoSystems systems)
        {
            _startWaitingOrderIt = new(new[]
            {
                typeof(GuestTag), typeof(ReachedTargetPositionEvent)
            }, new[] { typeof(WaitingOrderTag) });

            _startWaitingOrderIt.Init(_world);
        }

        public void Run()
        {
            foreach (var guestEntity in _startWaitingOrderIt)
            {
                Debug.Log("Старт ожидания");
                _guestAspect.GuestIsWalkingTagPool.DelIfExists(guestEntity);
                _guestAspect.NeedsTableTagPool.DelIfExists(guestEntity);
                _guestAspect.WaitingOrderTagPool.Add(guestEntity);
                ref var timer = ref _baseAspect.TimerPool.Add(guestEntity);
                timer.Duration = 10f;
            }
        }

        public void Destroy()
        {
            //throw new System.NotImplementedException();
        }
    }
}