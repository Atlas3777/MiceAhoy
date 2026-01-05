using System.Threading;
using Cysharp.Threading.Tasks;
using VContainer;

namespace Game.Scripts.TutorialTasks
{
    public abstract class TutorialTask
    {
        public abstract string Description { get; }

        public abstract UniTask Execute(
            IObjectResolver resolver,
            CancellationToken ct);
    }
}