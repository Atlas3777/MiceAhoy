using System;
using Cysharp.Threading.Tasks;
using Game.Scripts.Infrastructure;
using VContainer.Unity;

public class PausePresenter : IInitializable, IDisposable
{
    private readonly IPauseService _pauseService;
    private readonly PauseView _view;
    private readonly InputService _input;
    private readonly SceneController _sceneController;

    public PausePresenter(IPauseService pauseService, PauseView view, InputService input,
        SceneController sceneController)
    {
        _pauseService = pauseService;
        _view = view;
        _input = input;
        _sceneController = sceneController;
    }

    public void Initialize()
    {
        _input.OnPausePressed += TogglePause;

        _view.OnResumeRequested += HandleResumeRequested;
        _view.OnRetryRequested += HandleRetryRequested;
        _view.OnMainMenuRequested += HandleMainMenuRequested;

        _pauseService.OnPauseChanged += HandlePauseChanged;
    }

    private void TogglePause() => _pauseService.SetPause(!_pauseService.IsPaused);
    private void HandleResumeRequested() => _pauseService.SetPause(false);
    private void HandleRetryRequested() => _sceneController.LoadMainGameSceneAsync().Forget();
    private void HandleMainMenuRequested() => _sceneController.LoadMainMenuSceneAsync().Forget();

    private void HandlePauseChanged(bool isPaused)
    {
        if (isPaused)
            _view.Show();
        else _view.Hide();
    }

    public void Dispose()
    {
        // Полная очистка всех подписок
        _input.OnPausePressed -= TogglePause;

        _view.OnResumeRequested -= HandleResumeRequested;
        _view.OnRetryRequested -= HandleRetryRequested;
        _view.OnMainMenuRequested -= HandleMainMenuRequested;

        _pauseService.OnPauseChanged -= HandlePauseChanged;
    }
}