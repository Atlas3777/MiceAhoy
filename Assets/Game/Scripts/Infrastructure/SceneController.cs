using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game.Scripts.Infrastructure
{
    public class SceneController
    {
        public async UniTask ReloadSceneAsync()
        {
            var currentIndex = SceneManager.GetActiveScene().buildIndex;
            await SceneManager.LoadSceneAsync(currentIndex);
        }

        public void LoadMainGameScene()
        {
            Debug.Log("Loading Main Game Scene");
            Time.timeScale = 1;
            SceneManager.LoadScene("Game/Scenes/Main");
        }

        public void LoadTutorialScene()
        {
            Time.timeScale = 1;
            SceneManager.LoadScene("Game/Scenes/Tutorial");
        }

        public async UniTask LoadMainMenuScene()
        {
            Time.timeScale = 1;
            await SceneManager.LoadSceneAsync("Game/Scenes/Main Menu");
        }

        public void ExitAppication()
        {
            Application.Quit();
        }
    }
}