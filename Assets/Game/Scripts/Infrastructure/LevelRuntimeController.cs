using System;
using Leopotam.EcsProto;
using UnityEngine;
using VContainer.Unity;

public class LevelRuntimeController : IFixedTickable, IDisposable
{
    private readonly IProtoSystems _systems;
    private readonly InputService _input;
    private readonly PauseView _view;

    public bool IsPaused { get; private set; }

    public LevelRuntimeController(IProtoSystems systems, InputService input, PauseView view)
    {
        _systems = systems;
        _input = input;
        _view = view;
    }

    public void Start()
    {
        _systems.Init();
        
        _input.OnPausePressed += TogglePause;
        _view.OnResumeRequested += Unpause;
        _view.OnRetryRequested += HandleRetry;
        _view.OnMainMenuRequested += HandleMainMenu;
    }

    private void TogglePause()
    {
        if (IsPaused) Unpause();
        else Pause();
    }

    private void Pause()
    {
        IsPaused = true;
        Time.timeScale = 0;
        _input.SwitchAllActionMapsTo("UI");
        _view.Show();
    }

    private void Unpause()
    {
        IsPaused = false;
        Time.timeScale = 1;
        _input.SwitchAllActionMapsTo("Player");
        _view.Hide();
    }

    private void HandleRetry()
    {
        Time.timeScale = 1; 
        Debug.Log("Restarting level...");
    }

    private void HandleMainMenu()
    {
        Time.timeScale = 1;
    }

    public void FixedTick()
    {
        if (!IsPaused) _systems.Run();
    }

    public void Dispose()
    {
        _input.OnPausePressed -= TogglePause;
        _view.OnResumeRequested -= Unpause;
        _view.OnRetryRequested -= HandleRetry;
        _view.OnMainMenuRequested -= HandleMainMenu;
        
        _systems.World().Destroy();
        _systems.Destroy();
    }
}