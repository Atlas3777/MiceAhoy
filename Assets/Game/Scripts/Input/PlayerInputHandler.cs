using UnityEngine;
using UnityEngine.InputSystem;
using VContainer;


[RequireComponent(typeof(PlayerInput))]
public class PlayerInputHandler : MonoBehaviour
{
    private PlayerInput _playerInput;
    private int _playerIndex;
    private bool _isPressHandled = false;
    
    [Inject] private InputService _inputService;

    private void Awake()
    {
        _playerInput = GetComponent<PlayerInput>();
        _playerIndex = _playerInput.playerIndex;
        _inputService.RegisterPlayer(_playerIndex, _playerInput);

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
        var state = _inputService.GetPlayerInputState(_playerIndex);

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
            case "RandomSpawnFurniture":
                OnEdit(context, ref state);
                break;
            case "Pause":
                OnPause(context, ref state);
                break;
            case "GoRight":
                OnRightPressed(context, ref state);
                break;
            case "GoLeft":
                OnLeftPressed(context, ref state);
                break;
        }

        _inputService.UpdateState(_playerIndex, state);
    }

    private void OnPause(InputAction.CallbackContext context, ref InputService.PlayerInputData state)
    {
        // 1. Проверяем, что кнопка нажата и мы еще не обработали это нажатие
        if (context.performed && !_isPressHandled)
        {
            _isPressHandled = true; // Блокируем дальнейшие срабатывания
            _inputService.OnPausePressed.Invoke();
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

    private void OnRightPressed(InputAction.CallbackContext context, ref InputService.PlayerInputData state)
    {
        if (context.performed)
        {
            state.RightPressed = true;
        }
    }

    private void OnLeftPressed(InputAction.CallbackContext context, ref InputService.PlayerInputData state)
    {
        if (context.performed)
        {
            state.LeftPressed = true;
        }
    }

    private void OnDisable()
    {
        _playerInput.onActionTriggered -= HandleInput;
    }
}