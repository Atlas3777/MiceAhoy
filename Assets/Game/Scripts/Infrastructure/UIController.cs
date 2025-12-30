using System;
using Febucci.UI;
using Game.Script.Infrastructure;
using Game.Scripts;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

public class UIController : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private Button exitToMainMenuButton;

    [Header("Task HUD")]
    [SerializeField] private GameObject activeTask;
    private TMP_Text _taskText;
    private TypewriterByCharacter _taskTypewriter;

    [Header("Completed HUD")]
    [SerializeField] private GameObject completedTaskWindow; // окно/плашка с "Выполнено"
    private TypewriterByCharacter _completedTaskWindowTypewriter;

    private SceneController _sceneController;
    private TutorialTaskManager _tutorialTaskManager;

    // состояния печати
    private bool _isTaskTyping = false;
    private bool _isCompletedTyping = false;
    
    private bool _tutorialFinished = false;

    // когда менеджер пометил задачу как выполненную — UI ставит этот флаг и ждёт,
    // когда оба typewriter'а перестанут печатать, чтобы вызвать ProceedToNextTask.
    private bool _advanceRequestedByManager = false;

    private void Awake()
    {
        _taskText = activeTask.GetComponentInChildren<TMP_Text>();
        _taskTypewriter = activeTask.GetComponentInChildren<TypewriterByCharacter>();

        _completedTaskWindowTypewriter = completedTaskWindow.GetComponentInChildren<TypewriterByCharacter>();

        // начально скрываем окно выполнено
        if (completedTaskWindow != null)
            completedTaskWindow.SetActive(false);
    }

    [Inject]
    private void Initialize(SceneController sceneController, TutorialTaskManager tutorialTaskManager)
    {
        _sceneController = sceneController;
        _tutorialTaskManager = tutorialTaskManager;

        _tutorialTaskManager.OnTaskStart += UpdateTaskText;
        _tutorialTaskManager.OnAllTasksCompleted += AllTasksCompleted;
        _tutorialTaskManager.OnTaskComplete += OnTaskMarkedCompleteByManager;

        // Подпишемся на события завершения печати у typewriter'ов
        if (_taskTypewriter)
            _taskTypewriter.onTextShowed.AddListener(OnTaskTextFullyShown);

        if (_completedTaskWindowTypewriter)
            _completedTaskWindowTypewriter.onTextShowed.AddListener(OnCompletedTextFullyShown);

        Debug.Log("TutorialTaskManager и UIController инициализированы");
    }

    private void Start()
    {
        // exitToMainMenuButton.onClick.AddListener(_sceneController.LoadMainMenuScene);
    }

    private void OnDestroy()
    {
        if (_tutorialTaskManager != null)
        {
            _tutorialTaskManager.OnTaskStart -= UpdateTaskText;
            _tutorialTaskManager.OnAllTasksCompleted -= AllTasksCompleted;
            _tutorialTaskManager.OnTaskComplete -= OnTaskMarkedCompleteByManager;
        }

        if (_taskTypewriter)
            _taskTypewriter.onTextShowed.RemoveListener(OnTaskTextFullyShown);

        if (_completedTaskWindowTypewriter)
            _completedTaskWindowTypewriter.onTextShowed.RemoveListener(OnCompletedTextFullyShown);
    }

    private void AllTasksCompleted()
    {
        _tutorialFinished = true;

        // сбрасываем всю логику ожидания перехода
        _advanceRequestedByManager = false;
        _isTaskTyping = false;
        _isCompletedTyping = false;

        // скрываем окно "выполнено"
        if (completedTaskWindow != null)
            completedTaskWindow.SetActive(false);

        //HideInlineCheckmark();

        // гарантированно показываем финальный текст
        if (!activeTask.activeSelf)
            activeTask.SetActive(true);

        _taskText.text = "Все задачи выполнены";

        // если typewriter настроен на автоматический старт, иначе явно запускаем
        if (_taskTypewriter != null && !_taskTypewriter.isShowingText)
        {
            _taskTypewriter.StartShowingText();
        }

        _isTaskTyping = true;
    }

    // Менеджер вызвал этот эвент — задача помечена как выполненной.
    // UI показывает окно "Выполнено" (или галочку) и ждёт окончания печати.
    private void OnTaskMarkedCompleteByManager()
    {
        //ShowInlineCheckmark();

        if (completedTaskWindow != null)
        {
            completedTaskWindow.SetActive(true);

            if (_completedTaskWindowTypewriter != null)
            {
                if (!_completedTaskWindowTypewriter.isShowingText)
                {
                    _completedTaskWindowTypewriter.StartShowingText();
                }
                _isCompletedTyping = true;
            }
            else
            {
                _isCompletedTyping = false;
            }
        }

        _advanceRequestedByManager = true;
        TryAdvanceIfReady();
    }

    public void UpdateTaskText(string taskText)
    {
        if (!activeTask.activeSelf)
            activeTask.SetActive(true);

        // Сбрасываем возможные состояния предыдущей комплит-плашки
        if (completedTaskWindow != null)
            completedTaskWindow.SetActive(false);

        //HideInlineCheckmark();

        _taskText.text = taskText;

        // явно запускаем typewriter, если не стартует автоматически
        if (_taskTypewriter != null && !_taskTypewriter.isShowingText)
        {
            _taskTypewriter.StartShowingText();
        }

        _isTaskTyping = true;
    }

    private void OnTaskTextFullyShown()
    {
        _isTaskTyping = false;

        if (_tutorialFinished)
            return;

        TryAdvanceIfReady();
    }

    private void OnCompletedTextFullyShown()
    {
        _isCompletedTyping = false;
        TryAdvanceIfReady();
    }

    private void TryAdvanceIfReady()
    {
        if (!_advanceRequestedByManager)
            return;

        if (_isTaskTyping || _isCompletedTyping)
            return;

        _advanceRequestedByManager = false;

        if (completedTaskWindow != null)
            completedTaskWindow.SetActive(false);

        //HideInlineCheckmark();

        _tutorialTaskManager.ProceedToNextTask();
    }

    // private void ShowInlineCheckmark()
    // {
    //     var check = activeTask.transform.Find("CheckIcon");
    //     if (check != null) check.gameObject.SetActive(true);
    // }
    //
    // private void HideInlineCheckmark()
    // {
    //     var check = activeTask.transform.Find("CheckIcon");
    //     if (check != null) check.gameObject.SetActive(false);
    // }

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