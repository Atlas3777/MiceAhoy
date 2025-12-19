using UnityEngine;
using UnityEngine.InputSystem;


[RequireComponent(typeof(PlayerInput))]
public class PlayerInputHandler : MonoBehaviour
{
    private PlayerInput _playerInput;
    private int _playerIndex;
    private bool _isPressHandled = false;

    private void Awake()
    {
        _playerInput = GetComponent<PlayerInput>();
        _playerIndex = _playerInput.playerIndex;
        InputService.Instance.RegisterPlayer(_playerIndex, _playerInput);
    }

    private void Start()
    {
        
    }

    private void OnEnable()
    {
        _playerInput.onActionTriggered += HandleInput;
    }

    private void HandleInput(InputAction.CallbackContext context)
    {
        var state = InputService.Instance.GetPlayerInputState(_playerIndex);

        switch (context.action.name)
        {
            case "Move":
                OnMove(context, ref state);
                break;
            case "PickPlace":
                OnPickPlace(context, ref state);
                break;
            case "Interact":
                OnInteract(context, ref state);
                break;
            case "Edit":
                OnEdit(context, ref state);
                break;
            case "Pause":
                OnPause(context, ref state);
                break;
        }

        InputService.Instance.UpdateState(_playerIndex, state);
    }

    private void OnPause(InputAction.CallbackContext context, ref InputService.PlayerInputData state)
    {
        // 1. Проверяем, что кнопка нажата и мы еще не обработали это нажатие
        if (context.performed && !_isPressHandled)
        {
            _isPressHandled = true; // Блокируем дальнейшие срабатывания
            InputService.Instance.OnPausePressed.Invoke();
        }
    
        // 2. Сбрасываем флаг, когда кнопка отпущена
        if (context.canceled) 
        {
            _isPressHandled = false; // Разрешаем следующее нажатие
        }
    }

    private void OnMove(InputAction.CallbackContext context, ref InputService.PlayerInputData state)
    {
        state.MoveDirection = context.ReadValue<Vector2>();
    }

    private void OnPickPlace(InputAction.CallbackContext context, ref InputService.PlayerInputData state)
    {
        if (context.performed)
        {
            state.PickPlacePressed = true;
        }
    }

    private void OnInteract(InputAction.CallbackContext context, ref InputService.PlayerInputData state)
    {
        if (context.performed)
        {
            state.InteractPressed = true;
        }
    }

    private void OnEdit(InputAction.CallbackContext context, ref InputService.PlayerInputData state)
    {
        if (context.performed)
        {
            state.RandomSpawnFurniturePressed = true;
        }
    }

    private void OnDisable()
    {
        _playerInput.onActionTriggered -= HandleInput;
    }
}