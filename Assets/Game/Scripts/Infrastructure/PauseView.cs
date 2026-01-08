using System;
using UnityEngine;
using UnityEngine.UI;

public class PauseView : MonoBehaviour
{
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private Button resumeButton;
    [SerializeField] private Button retryButton;
    [SerializeField] private Button menuButton;

    public event Action OnResumeRequested;
    public event Action OnRetryRequested;
    public event Action OnMainMenuRequested;

    private void Awake()
    {
        resumeButton.onClick.AddListener(() => OnResumeRequested?.Invoke());
        retryButton.onClick.AddListener(() => OnRetryRequested?.Invoke());
        menuButton.onClick.AddListener(() => OnMainMenuRequested?.Invoke());
    }

    public void Show()
    {
        canvasGroup.gameObject.SetActive(true);
        canvasGroup.alpha = 1;
        canvasGroup.blocksRaycasts = true;
    }

    public void Hide()
    {
        canvasGroup.alpha = 0;
        canvasGroup.blocksRaycasts = false;
        canvasGroup.gameObject.SetActive(false);
    }
}