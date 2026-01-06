using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.InputSystem;
using VContainer.Unity;

public class InputService : ILateTickable
{
    public struct PlayerInputData
    {
        public Vector2 MoveDirection;
        public bool InteractPressed;
        public bool PickPlacePressed;
        public bool PlacementModePressed;
        public bool OpenScrollPressed;
        public bool LeftPressed;
        public bool RightPressed;
    }

    private Dictionary<int, PlayerInput> _playerComponents = new();
    private Dictionary<int, PlayerInputData> _playerInputs = new();

    private Queue<int> _pendingPlayerIndices = new();

    public Action OnPausePressed;

    public void RegisterPlayer(int playerIndex, PlayerInput playerInput)
    {
        if (!_playerInputs.ContainsKey(playerIndex))
        {
            _playerInputs[playerIndex] = new PlayerInputData();
            _playerComponents[playerIndex] = playerInput;
            _pendingPlayerIndices.Enqueue(playerIndex);
            Debug.Log($"Player {playerIndex} registered in InputService.");
        }
    }

    public void SwitchAllActionMapsTo(string mapName)
    {
        Debug.Log($"Switching all players to Action Map: {mapName}");
        foreach (var playerInput in _playerComponents.Values)
        {
            if (playerInput != null)
            {
                playerInput.SwitchCurrentActionMap(mapName);
            }
        }
    }

    public bool TryGetPendingPlayerIndex(out int index)
    {
        if (_pendingPlayerIndices.Count > 0)
        {
            index = _pendingPlayerIndices.Dequeue();
            return true;
        }

        index = -1;
        return false;
    }

    public void UpdateState(int playerIndex, PlayerInputData newData)
    {
        // *** ИСПРАВЛЕНИЕ: Удаляем некорректную логику регистрации ***
        if (!_playerInputs.ContainsKey(playerIndex))
        {
            // Этот код должен вызываться только для зарегистрированных игроков.
            // Если он сработал, значит, компонент PlayerInput не был зарегистрирован вовремя.
            Debug.LogError($"InputService: Attempted to UpdateState for unregistered player index {playerIndex}. Registration must happen first");
            return;
        }

        var currentData = _playerInputs[playerIndex];
        
        // Обновляем состояние только если нажатия произошли (поведение "одноразового нажатия")
        if (newData.InteractPressed) currentData.InteractPressed = true;
        if (newData.PickPlacePressed) currentData.PickPlacePressed = true;
        if (newData.PlacementModePressed) currentData.PlacementModePressed = true;
        if (newData.OpenScrollPressed) currentData.OpenScrollPressed = true;
        if (newData.LeftPressed) currentData.LeftPressed = true;
        if (newData.RightPressed) currentData.RightPressed = true;


        currentData.MoveDirection = newData.MoveDirection;
        
        _playerInputs[playerIndex] = currentData;
    }

    public PlayerInputData GetPlayerInputState(int playerIndex)
    {
        if (_playerInputs.TryGetValue(playerIndex, out var state))
        {
            return state;
        }

        // Возвращаем пустые данные, если игрок не найден
        return new PlayerInputData();
    }

    public int CountActivePlayerIndices() => _playerInputs.Count;

    public void LateTick()
    {
        var keys = _playerInputs.Keys.ToList();
        foreach (var playerIndex in keys)
        {
            var state = _playerInputs[playerIndex];
            state.InteractPressed = false;
            state.PickPlacePressed = false;
            state.PlacementModePressed = false;
            state.OpenScrollPressed = false;
            state.RightPressed = false;
            state.LeftPressed = false;
            state.OpenScrollPressed = false;
            _playerInputs[playerIndex] = state;
        }
    }
}