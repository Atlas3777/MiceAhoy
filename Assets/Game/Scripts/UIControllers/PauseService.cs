using System;
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
    private float _lastToggleTime;
    private const float ToggleCooldown = 0.15f; // Задержка в секундах

    public bool IsPaused { get; private set; }
    public event Action<bool> OnPauseChanged;

    public PauseService(InputService input) => _input = input;

    public void SetPause(bool isPaused)
    {
        // 1. Проверка на то же самое состояние
        if (IsPaused == isPaused) return;

        // 2. Защита от мгновенного переключения (используем Realtime, так как TimeScale будет 0)
        if (Time.unscaledTime - _lastToggleTime < ToggleCooldown) 
            return;

        _lastToggleTime = Time.unscaledTime;
        IsPaused = isPaused;

        // Логика паузы
        Time.timeScale = isPaused ? 0 : 1;
        _input.SwitchAllActionMapsTo(isPaused ? "UI" : "Player");
        
        OnPauseChanged?.Invoke(IsPaused);
    }
}