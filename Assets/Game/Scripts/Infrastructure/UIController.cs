using Game.Script.Infrastructure;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

public class UIController : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private Button exitToMainMenuButton;
    [SerializeField] private GameObject gameResultWin;
    [SerializeField] private GameObject gameResultLose;
    [SerializeField] private TMP_Text DayCountText;
        
    private SceneController _sceneController;

    [Inject]
    private void Initialize(IObjectResolver objectResolver)
    {
        _sceneController = objectResolver.Resolve<SceneController>();
    }
        
    private void Start()
    {
        //exitToMainMenuButton.onClick.AddListener(_sceneController.LoadMainMenuScene);
    }

    public void OpenPauseMenu()
    {
        Debug.Log("Opening pause menu");
        pauseMenu.SetActive(true);
    }

    public void ClosePauseMenu()
    {
        pauseMenu.SetActive(false);
    }

    public void ShowLose()
    {
        gameResultLose.SetActive(true);
    }

    public void ShowWin()
    {
        gameResultWin.SetActive(true);
    }
}