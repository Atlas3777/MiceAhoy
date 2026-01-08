using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.InputSystem;
using VContainer.Unity;

public class InputService : IPostFixedTickable
{
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
        if (!_playerInputs.ContainsKey(playerIndex))
        {
            Debug.LogError($"[INPUT] UpdateState: Player {playerIndex} NOT REGISTERED!");
            return;
        }

        var currentData = _playerInputs[playerIndex];

        if (newData.InteractPressed) currentData.InteractPressed = true;
        if (newData.PickPlacePressed) currentData.PickPlacePressed = true;
        currentData.MoveDirection = newData.MoveDirection;

        _playerInputs[playerIndex] = currentData;
    }

    public PlayerInputData GetPlayerInputState(int playerIndex)
    {
        if (_playerInputs.TryGetValue(playerIndex, out var state))
        {
            return state;
        }

        return new PlayerInputData();
    }

    public int CountActivePlayerIndices() => _playerInputs.Count;


    public void PostFixedTick()
    {
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