using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.InputSystem;

public class InputService : MonoBehaviour
{
    public static InputService Instance { get; private set; }

    public struct PlayerInputData
    {
        public Vector2 MoveDirection;
        public bool InteractPressed;
        public bool PickPlacePressed;
        public bool RandomSpawnFurniturePressed;
        public bool MoveFurniturePressed;
    }

    private Dictionary<int, PlayerInput> _playerComponents = new();
    private Dictionary<int, PlayerInputData> _playerInputs = new();

    private Queue<int> _pendingPlayerIndices = new();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

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
            Debug.LogError($"InputService: Attempted to UpdateState for unregistered player index {playerIndex}. Registration must happen first (e.g., in PlayerInputHandler's Start or OnPlayerJoined).");
            return;
        }

        var currentData = _playerInputs[playerIndex];
        
        // Обновляем состояние только если нажатия произошли (поведение "одноразового нажатия")
        if (newData.InteractPressed) currentData.InteractPressed = true;
        if (newData.PickPlacePressed) currentData.PickPlacePressed = true;
        if (newData.RandomSpawnFurniturePressed) currentData.RandomSpawnFurniturePressed = true;
        if (newData.MoveFurniturePressed) currentData.MoveFurniturePressed = true;
        
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

    private void LateUpdate()
    {
        // Сброс булевых флагов нажатий после обработки в конце кадра
        var keys = _playerInputs.Keys.ToList();
        foreach (var playerIndex in keys)
        {
            var state = _playerInputs[playerIndex];
            state.InteractPressed = false;
            state.PickPlacePressed = false;
            state.RandomSpawnFurniturePressed = false;
            state.MoveFurniturePressed = false;
            _playerInputs[playerIndex] = state;
        }
    }
}