using System;
using Game.Script.Systems;
using VContainer;

namespace Game.Scripts.TutorialTasks
{
    [Serializable]
    public class PickTask : TutorialTask
    {
        public override string Description => "TEST-TEST поднимите предмет";
    
        private PickPlaceSystem _pickPlaceSystem;

        public override void Enter(IObjectResolver resolver)
        {
            if (resolver.TryResolve<PickPlaceSystem>(out _pickPlaceSystem))
                _pickPlaceSystem.PlayerPick += HandlePick;
            else
                UnityEngine.Debug.LogError("PickTask: PickPlaceSystem не найден в контейнере!");
        }

        private void HandlePick()
        {
            NotifyComplete();
        }

        public override void Exit()
        {
            if (_pickPlaceSystem != null)
            {
                _pickPlaceSystem.PlayerPick -= HandlePick;
            }
        }
    }
}