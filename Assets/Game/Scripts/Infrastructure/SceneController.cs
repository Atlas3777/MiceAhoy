using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game.Scripts.Infrastructure
{
    public class SceneController
    {
        private const string MainMenuSceneName = "MainMenu";
        private const string MainScenePath = "MainLevel";
        private const string ShopScenePath = "Shop";
        private const string TutorialScenePath = "Tutorial";

        // Теперь все методы возвращают UniTask
        public async UniTask ReloadSceneAsync() => await LoadSceneAsync(SceneManager.GetActiveScene().name);
        public async UniTask LoadMainGameSceneAsync() => await LoadSceneAsync(MainScenePath);
        public async UniTask LoadTutorialSceneAsync() => await LoadSceneAsync(TutorialScenePath);
        public async UniTask LoadShopSceneAsync() => await LoadSceneAsync(ShopScenePath);
        public async UniTask LoadMainMenuSceneAsync() => await LoadSceneAsync(MainMenuSceneName);

        public void ExitApplication()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }

        private async UniTask LoadSceneAsync(string sceneName)
        {
            Debug.Log($"[SceneController] Loading: {sceneName}");
            ResetTime();
            
            await SceneManager.LoadSceneAsync(sceneName).ToUniTask();
            
            Debug.Log($"[SceneController] {sceneName} loaded successfully.");
        }

        private void ResetTime() => Time.timeScale = 1;
    }
}