using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Scripts.UIControllers
{
    public enum LoseButtonResult { Retry, MainMenu }
    public class LoseUIController : MonoBehaviour
    {
        [SerializeField] private CanvasGroup canvasGroup;
        [SerializeField] private Button retryButton;
        [SerializeField] private Button menuButton;

        public async UniTask<LoseButtonResult> WaitForChoice(CancellationToken ct) 
        {
            var tcs = new UniTaskCompletionSource<LoseButtonResult>();

            retryButton.onClick.AddListener(() => tcs.TrySetResult(LoseButtonResult.Retry));
            menuButton.onClick.AddListener(() => tcs.TrySetResult(LoseButtonResult.MainMenu));

            canvasGroup.gameObject.SetActive(true);
            canvasGroup.alpha = 1;

            try 
            {
                return await tcs.Task.AttachExternalCancellation(ct);
            }
            finally 
            {
                retryButton.onClick.RemoveAllListeners();
                menuButton.onClick.RemoveAllListeners();
                
                canvasGroup.alpha = 0;
                canvasGroup.gameObject.SetActive(false);
            }
        }
    }
}