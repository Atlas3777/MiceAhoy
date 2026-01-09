using Game.Script.Aspects;
using Game.Scripts.Aspects;
using Leopotam.EcsProto;
using Leopotam.EcsProto.QoL;
using UnityEngine;

namespace Game.Scripts.Systems
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
            }, new[] { typeof(WaitingOrderTag), typeof(GuestServicedTag) });

            _startWaitingOrderIt.Init(_world);
        }

        public void Run()
        {
            foreach (var guestEntity in _startWaitingOrderIt)
            {
                Debug.Log("Старт ожидания");
                _guestAspect.GuestIsWalkingTagPool.DelIfExists(guestEntity);
                _guestAspect.NeedsTableTagPool.DelIfExists(guestEntity);
                
                _guestAspect.GuestViewComponentPool.Get(guestEntity).view.canvasGroup.alpha = 1;
                
                
                _guestAspect.WaitingOrderTagPool.Add(guestEntity);
                ref var timer = ref _baseAspect.TimerPool.GetOrAdd(guestEntity);
                ref var guestState = ref _guestAspect.GuestStateComponentPool.GetOrAdd(guestEntity);
                
                timer.Duration = guestState.WaitingSeconds;
            }
        }

        public void Destroy()
        {
            //throw new System.NotImplementedException();
        }
    }
}