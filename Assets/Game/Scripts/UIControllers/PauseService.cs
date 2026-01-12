using System;
using Game.Scripts;
using UnityEngine;

public interface IPauseService
{
    bool IsPaused { get; }
    void SetPause(bool isPaused);
    event Action<bool> OnPauseChanged;
}

public class PauseService : IPauseService
{
    private readonly InputService _input;
    private readonly SoundManager _soundManager;
    private float _lastToggleTime;
    private const float ToggleCooldown = 0.15f;

    public bool IsPaused { get; private set; }
    public event Action<bool> OnPauseChanged;

    public PauseService(InputService input, SoundManager soundManager)
    {
        _input = input;
        _soundManager = soundManager;
    }

    public void SetPause(bool isPaused)
    {
        if (IsPaused == isPaused) return;

        if (Time.unscaledTime - _lastToggleTime < ToggleCooldown)
            return;

        _lastToggleTime = Time.unscaledTime;
        IsPaused = isPaused;

        Time.timeScale = isPaused ? 0 : 1;
        _soundManager.SetPause(isPaused);
        _input.SwitchAllActionMapsTo(isPaused ? "UI" : "Player");

        OnPauseChanged?.Invoke(IsPaused);
    }
}