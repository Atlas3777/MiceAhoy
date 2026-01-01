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

    [Header("Task HUD")] [SerializeField] private GameObject activeTask;
    [SerializeField] private TMP_Text taskText;
    [SerializeField] private TypewriterByCharacter taskTypewriter;

    [Header("Completed HUD")] [SerializeField]
    private GameObject completedTaskWindow;
    [SerializeField] private TMP_Text taskCompileText;
    [SerializeField] private TypewriterByCharacter completedTaskWindowTypewriter;

    private SceneController _sceneController;
    private TutorialTaskManager _tutorialTaskManager;

    private bool _isTaskTyping = false;
    private bool _isCompletedTextTyping = false;

    //private bool _tutorialFinished = false;

    private bool _advanceRequestedByManager = false;

    private void Awake()
    {
        // taskText = activeTask.GetComponentInChildren<TMP_Text>();
        // taskTypewriter = activeTask.GetComponentInChildren<TypewriterByCharacter>();
        //
        // completedTaskWindowTypewriter = completedTaskWindow.GetComponentInChildren<TypewriterByCharacter>();

        if (completedTaskWindow)
            completedTaskWindow.SetActive(false);
    }

    //[Inject]
    private void Initialize(SceneController sceneController, TutorialTaskManager tutorialTaskManager)
    {
        _sceneController = sceneController;
        _tutorialTaskManager = tutorialTaskManager;

        _tutorialTaskManager.TaskStart += UpdateTaskText;
        _tutorialTaskManager.AllTasksCompleted += AllTasksCompleted;
        _tutorialTaskManager.TaskComplete += TaskMarkedCompleteByManager;

        if (taskTypewriter)
        {
            Debug.Log("Tutorial task typewriter");
            taskTypewriter.onTextShowed.AddListener(OnTaskTextFullyShown);
        }

        if (completedTaskWindowTypewriter)
        {
            Debug.Log("Tutorial completed task window");
            completedTaskWindowTypewriter.onTextShowed.AddListener(OnCompletedTextFullyShown);
        }

        Debug.Log("TutorialTaskManager и UIController инициализированы");
    }

    private void OnDestroy()
    {
        if (_tutorialTaskManager != null)
        {
            _tutorialTaskManager.TaskStart -= UpdateTaskText;
            _tutorialTaskManager.AllTasksCompleted -= AllTasksCompleted;
            _tutorialTaskManager.TaskComplete -= TaskMarkedCompleteByManager;
        }

        if (taskTypewriter)
            taskTypewriter.onTextShowed.RemoveListener(OnTaskTextFullyShown);

        if (completedTaskWindowTypewriter)
            completedTaskWindowTypewriter.onTextShowed.RemoveListener(OnCompletedTextFullyShown);
    }

    private void AllTasksCompleted()
    {
        //_tutorialFinished = true;

        _advanceRequestedByManager = false;
        _isTaskTyping = false;
        _isCompletedTextTyping = false;

        if (completedTaskWindow != null)
            completedTaskWindow.SetActive(false);

        if (!activeTask.activeSelf)
            activeTask.SetActive(true);

        taskText.text = "Все задачи выполнены";

        if (taskTypewriter != null && !taskTypewriter.isShowingText)
        {
            taskTypewriter.StartShowingText();
        }

        _isTaskTyping = true;
    }

    private void TaskMarkedCompleteByManager()
    {
        if (!completedTaskWindow) return;
        completedTaskWindow.SetActive(true);
        taskCompileText.text = "Выполнено";

        if (completedTaskWindowTypewriter)
        {
            if (!completedTaskWindowTypewriter.isShowingText)
            {
                completedTaskWindowTypewriter.StartShowingText();
            }
            _isCompletedTextTyping = true;
        }
        else
            _isCompletedTextTyping = false;

        _advanceRequestedByManager = true;
        TryAdvanceIfReady();
    }

    public void UpdateTaskText(string taskText)
    {
        if (!activeTask.activeSelf)
            activeTask.SetActive(true);

        if (completedTaskWindow)
            completedTaskWindow.SetActive(false);

        this.taskText.text = taskText;
        taskTypewriter.StartShowingText();
        _isTaskTyping = true;
    }

    private void OnTaskTextFullyShown()
    {
        Debug.Log("OnTaskTextFullyShown");
        _isTaskTyping = false;
        TryAdvanceIfReady();
    }

    private void OnCompletedTextFullyShown()
    {
        _isCompletedTextTyping = false;
        TryAdvanceIfReady();
    }

    private void TryAdvanceIfReady()
    {
        if (!_advanceRequestedByManager)
            return;

        if (_isTaskTyping || _isCompletedTextTyping)
            return;

        _advanceRequestedByManager = false;

        if (completedTaskWindow)
            completedTaskWindow.SetActive(false);

        _tutorialTaskManager.ProceedToNextTask();
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
}