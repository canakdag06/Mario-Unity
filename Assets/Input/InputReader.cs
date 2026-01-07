using System;
using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(menuName = "InputReader")]
public class InputReader : ScriptableObject, GameInput.IGameplayActions, GameInput.IUIActions
{
    public GameInput InputActions => gameInput;
    private GameInput gameInput;


    // GAMEPLAY CONTROLS
    public event Action<Vector2> MoveEvent;
    public event Action CrouchStartedEvent;
    public event Action CrouchCanceledEvent;
    public event Action JumpStartedEvent;
    public event Action JumpPerformedEvent;
    public event Action JumpCanceledEvent;
    public event Action FireEvent;
    public event Action SprintStartedEvent;
    public event Action SprintCanceledEvent;
    public event Action PauseEvent;


    // UI CONTROLS
    public event Action ResumeEvent;

    private void OnEnable()
    {
        if (gameInput == null)
        {
            gameInput = new GameInput();
            gameInput?.Enable();
            gameInput.Gameplay.SetCallbacks(this);          //SET CALLBACKS
            gameInput.UI.SetCallbacks(this);

            SetGameplay();
        }
    }

    private void OnDisable()
    {
        gameInput.Gameplay.Disable();
        gameInput.UI.Disable();
    }

    public void SetGameplay()
    {
        gameInput.UI.Disable();
        gameInput.Gameplay.Enable();
    }

    public void SetUI()
    {
        gameInput.Gameplay.Disable();
        gameInput.UI.Enable();
    }


    // ----------- GAMEPLAY CONTROLS ------------ //
    public void OnMove(InputAction.CallbackContext context)
    {
        MoveEvent?.Invoke(context.ReadValue<Vector2>());
    }

    public void OnCrouch(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
            CrouchStartedEvent?.Invoke();
        else if (context.phase == InputActionPhase.Canceled)
            CrouchCanceledEvent?.Invoke();
    }


    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
            JumpStartedEvent?.Invoke();
        else if (context.phase == InputActionPhase.Performed)
            JumpPerformedEvent?.Invoke();
        else if (context.phase == InputActionPhase.Canceled)
            JumpCanceledEvent?.Invoke();
    }

    public void OnFire(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
            FireEvent?.Invoke();
    }

    public void OnSprint(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
            SprintStartedEvent?.Invoke();
        else if (context.phase == InputActionPhase.Canceled)
            SprintCanceledEvent?.Invoke();
    }

    public void OnPause(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            PauseEvent?.Invoke();
            //SetUI();
        }
    }

    // ----------- UI CONTROLS ------------ //
    public void OnResume(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            PauseEvent?.Invoke();
            //SetGameplay();
        }
    }
    // ------------------------------------ //
}

