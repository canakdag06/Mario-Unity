using System;
using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(menuName = "InputReader")]
public class InputReader : ScriptableObject, GameInput.IGameplayActions, GameInput.IUIActions
{
    private GameInput gameInput;


    // GAMEPLAY CONTROLS
    public event Action<Vector2> MoveEvent;
    public event Action PauseEvent;

    // UI CONTROLS
    public event Action ResumeEvent;

    private void OnEnable()
    {
        if (gameInput == null)
        {
            gameInput = new GameInput();

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

    public void OnPause(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            PauseEvent?.Invoke();
            SetUI();
        }
    }

    // ----------- UI CONTROLS ------------ //
    public void OnResume(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            ResumeEvent?.Invoke();
            SetGameplay();
        }
    }
}

