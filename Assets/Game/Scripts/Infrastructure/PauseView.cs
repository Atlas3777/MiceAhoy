using System;
using Febucci.UI;
using Game.Script.Infrastructure;
using Game.Scripts;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using VContainer;

public class PauseView : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private Button exitToMainMenuButton;


    private SceneController _sceneController;

    [Inject]
    private void Initialize(SceneController sceneController)
    {
        _sceneController = sceneController;
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

    private void OnDestroy()
    {
    }
}