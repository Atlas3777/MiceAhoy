using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Febucci.UI;
using Game.Scripts;
using Game.Scripts.TutorialTasks;
using PrimeTween;
using UnityEngine;
using TMPro;
using VContainer;

public class TutorialUIController : MonoBehaviour
{
    [Header("Task")]
    [SerializeField] private CanvasGroup taskGroup;
    [SerializeField] private TypewriterByCharacter taskTypewriter;
    [SerializeField] private TMP_Text taskText;

    [Header("Complete")]
    [SerializeField] private CanvasGroup completedGroup;
    [SerializeField] private TMP_Text completedText;
    
    private TutorialTaskManager _manager;
    private CancellationTokenSource _taskCts;
    private UniTask _typingTask;

    [Inject]
    public void Construct(TutorialTaskManager manager)
    {
        _manager = manager;
        _manager.TaskStart += desc => OnTaskStarted(desc).Forget();
        _manager.TaskComplete += () => OnTaskCompleted().Forget();
        _manager.AllTasksCompleted += () => OnAllCompleted().Forget();
    }


    private async UniTaskVoid OnTaskStarted(string description)
    {
        _taskCts?.Cancel();
        _taskCts = new CancellationTokenSource();
        var ct = _taskCts.Token;
        
        taskGroup.gameObject.SetActive(true);
        taskGroup.alpha = 1;
    
        _typingTask = taskTypewriter.ShowTextAsync(description, ct).Preserve();
        await _typingTask;
    }

    private async UniTaskVoid OnTaskCompleted()
    {
        var ct = destroyCancellationToken;

        if (taskTypewriter.isShowingText)
        {
            taskTypewriter.SkipTypewriter();
            await _typingTask;
        }

        completedGroup.gameObject.SetActive(true);
        completedGroup.alpha = 1;

        completedText.text = "Выполнено";
        
        await Tween.Scale(completedGroup.transform, 0.5f, 1f, 0.6f, Ease.OutBack).ToUniTask(cancellationToken: ct);

        await UniTask.Delay(TimeSpan.FromSeconds(1.5f), cancellationToken: ct);

        await UniTask.WhenAll(
            Tween.Alpha(completedGroup, 1, 0, 0.5f).ToUniTask(cancellationToken: ct),
            Tween.Alpha(taskGroup, 1, 0, 0.5f).ToUniTask(cancellationToken: ct)
        );

        completedGroup.gameObject.SetActive(false);
        taskGroup.gameObject.SetActive(false);

        _manager.ProceedToNextTask();
    }
    
    private async UniTaskVoid OnAllCompleted()
    {
        taskText.text = "Все задачи выполнены";
        taskGroup.gameObject.SetActive(true);
        taskGroup.alpha = 1;
        await Tween.Scale(taskGroup.transform, 0.5f, 1f, 0.6f, Ease.OutBack).ToUniTask();
    }
}