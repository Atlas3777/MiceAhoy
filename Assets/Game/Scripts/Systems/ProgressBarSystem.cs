using Game.Scripts.Aspects;
using Leopotam.EcsProto;
using Leopotam.EcsProto.QoL;

namespace Game.Script.Systems
{
    public class ProgressBarSystem : IProtoInitSystem, IProtoRunSystem
    {
        [DI] readonly WorkstationsAspect _workstationsAspect;
        [DI] readonly ViewAspect _viewAspect;
        [DI] readonly BaseAspect _baseAspect;
        [DI] readonly ProtoWorld _world;

        private ProtoIt _runningIt;
        private ProtoIt _endIt;

        public void Init(IProtoSystems systems)
        {
            _runningIt = new(new[] { typeof(TimerComponent), typeof(ProgressBarComponent) });
            _endIt = new(new[] { typeof(TimerCompletedEvent), typeof(ProgressBarComponent) });
            _runningIt.Init(_world);
            _endIt.Init(_world);
        }

        public void Run()
        {
            foreach (var progressBarEntity in _runningIt)
            {
                ref var progressBar = ref _viewAspect.ProgressBarPool.Get(progressBarEntity);
                ref var timer = ref _baseAspect.TimerPool.Get(progressBarEntity);

                if (progressBar.IsActive)
                {
                    var progressValue = timer.Elapsed / timer.Duration;
                    progressBar.Image.fillAmount = 1 - progressValue;
                    progressBar.Image.color = progressBar.Gradient.Evaluate(progressValue);
                }
                else
                {
                    progressBar.ShowComponent();
                }
            }

            foreach (var progressBarEntity in _endIt)
            {
                ref var progressBar = ref _viewAspect.ProgressBarPool.Get(progressBarEntity);
                progressBar.HideComponent();
            }
        }
    }
}