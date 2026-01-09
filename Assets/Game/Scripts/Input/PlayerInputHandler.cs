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
    }


    private void OnEnable()
    {
        _playerInput.onActionTriggered += HandleInput;

        if (_playerInput.user.valid)
        {
            _inputService.RegisterPlayer(_playerIndex, _playerInput);
        }
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
            case "OpenScrollMenu":
                OnOpenScroll(context, ref state);
                break;
            case "TogglePlaceMode":
                OnPlacement(context, ref state);
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
            case "BackToPool":
                OnBackPressed(context, ref state);
                break;
        }

        _inputService.UpdateState(_playerIndex, state);
    }

    private void OnPause(InputAction.CallbackContext context, ref InputService.PlayerInputData state)
    {
        // Используем только performed. 
        // Когда карта сменится на UI, это действие "умрет" само по себе до следующего нажатия.
        if (context.performed)
        {
            _inputService.OnPausePressed?.Invoke();
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

    private void OnOpenScroll(InputAction.CallbackContext context, ref InputService.PlayerInputData state)
    {
        if (context.performed)
        {
            state.OpenScrollPressed = true;
        }
    }

    private void OnPlacement(InputAction.CallbackContext context, ref InputService.PlayerInputData state)
    {
        if (context.performed)
        {
            state.PlacementModePressed = true;
        }
    }

    private void OnBackPressed(InputAction.CallbackContext context, ref InputService.PlayerInputData state)
    {
        if (context.performed)
        {
            state.BackToPoolPressed = true;
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