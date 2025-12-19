using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game.Script.Infrastructure
{
    public class SceneController
    {
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
    
        public void LoadMainMenuScene()
        {
            Time.timeScale = 1;
            SceneManager.LoadScene("Game/Scenes/Main Menu");
        }
    
        public void LoadExitScene()
        {
            Application.Quit();
        }
    }
}