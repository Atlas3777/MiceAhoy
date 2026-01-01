using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Febucci.UI;
using Game.Scripts.TutorialTasks;
using PrimeTween;
using TMPro;
using UnityEngine;

public class TutorialUIController : MonoBehaviour
{
    [Header("Task")] [SerializeField] private CanvasGroup taskGroup;
    [SerializeField] private TypewriterByCharacter taskTypewriter;
    [SerializeField] private TMP_Text taskText;

    [Header("Complete")] [SerializeField] private CanvasGroup completedGroup;
    [SerializeField] private TMP_Text completedText;

    private UniTask _typingTask;


    public async UniTask ShowTaskAsync(string description, CancellationToken ct)
    {
        taskGroup.gameObject.SetActive(true);
        taskGroup.alpha = 1;

        taskText.text = string.Empty;
        _typingTask = taskTypewriter.ShowTextAsync(description, ct).Preserve();
        await _typingTask;
    }

    public async UniTask ShowCompletedAsync(CancellationToken ct)
    {
        if (taskTypewriter.isShowingText)
        {
            taskTypewriter.SkipTypewriter();
            await _typingTask;
        }

        completedGroup.gameObject.SetActive(true);
        completedGroup.alpha = 1;
        completedText.text = "Выполнено";

        await Tween.Scale(completedGroup.transform, 0.5f, 1f, 0.6f, Ease.OutBack)
            .ToUniTask(cancellationToken: ct);

        await UniTask.Delay(TimeSpan.FromSeconds(1.5f), cancellationToken: ct);

        await UniTask.WhenAll(
            Tween.Alpha(completedGroup, 1, 0, 0.5f).ToUniTask(ct),
            Tween.Alpha(taskGroup, 1, 0, 0.5f).ToUniTask(ct)
        );

        completedGroup.gameObject.SetActive(false);
        taskGroup.gameObject.SetActive(false);
    }

    public async UniTask ShowAllCompletedAsync(CancellationToken ct)
    {
        taskText.text = "Все задачи выполнены";
        taskGroup.gameObject.SetActive(true);
        taskGroup.alpha = 1;

        await Tween.Scale(taskGroup.transform, 0.5f, 1f, 0.6f, Ease.OutBack)
            .ToUniTask(cancellationToken: ct);
    }

    public async UniTask WaitForConfirmAsync(CancellationToken ct)
    {
        await UniTask.WaitUntil(
            () => Input.GetKeyDown(KeyCode.E),
            cancellationToken: ct
        );
    }
}