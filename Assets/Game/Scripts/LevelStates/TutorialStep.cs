using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Game.Scripts.TutorialTasks;
using Game.Scripts.TutorialTasks.Game.Scripts.TutorialTasks;
using VContainer;

namespace Game.Scripts.LevelStates
{
    [Serializable]
    public class TutorialState : LevelState
    {
        public override string Description => "Test";

        public override async UniTask Execute(IObjectResolver resolver, CancellationToken ct)
        {
            await new MoveStep().Execute(resolver, ct);
            await new PickStep().Execute(resolver, ct);
            await new GuestStep().Execute(resolver, ct);
        }
        
        public override void Exit()
        {
            
        }
    }
}