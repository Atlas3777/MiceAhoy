using System.Threading;
using Cysharp.Threading.Tasks;
using Febucci.UI;

namespace Game.Scripts.TutorialTasks
{
    public static class TypewriterExtensions
    {
        public static async UniTask ShowTextAsync(this TypewriterByCharacter typewriter, string text, CancellationToken ct)
        {
            // Создаем источник, который завершит задачу, когда событие сработает
            var utcs = new UniTaskCompletionSource();
        
            void OnComplete() => utcs.TrySetResult();
        
            typewriter.onTextShowed.AddListener(OnComplete);
            try
            {
                typewriter.ShowText(text);
                await utcs.Task.AttachExternalCancellation(ct);
            }
            finally
            {
                typewriter.onTextShowed.RemoveListener(OnComplete);
            }
        }
    }
}