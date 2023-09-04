using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(menuName = "Inputs/PlayerInput")]
public class PlayerInput : ScriptableObject, InputActions.IGamePlayActions
{
    InputActions inputActions;
    public event System.Action<Vector2> OnMoveEvent = delegate { };
    public event System.Action OnStopMoveEvent = delegate { };
    public event System.Action OnFireEvent = delegate { };
    public event System.Action OnStopFireEvent = delegate { };

    private void OnEnable() 
    {
        inputActions = new InputActions();
        inputActions.GamePlay.SetCallbacks(this);
    }

    private void OnDisable() 
    {
        DisableAllInputs();
    }

    private void SwitchActionMap(InputActionMap actionMap, bool isUIInput) 
    {
        inputActions.Disable();  // 現在のアクションマップを無効化する
        actionMap.Enable();  // 指定のアクションマップを有効化する

        if (isUIInput) 
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
        else 
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    public void Switch2DynamicUpdateMode() => InputSystem.settings.updateMode = InputSettings.UpdateMode.ProcessEventsInDynamicUpdate;
    public void Switch2FixedUpdateMode() => InputSystem.settings.updateMode = InputSettings.UpdateMode.ProcessEventsInFixedUpdate;

    public void DisableAllInputs() => inputActions.Disable();
    public void EnableGameplayInput() => SwitchActionMap(inputActions.GamePlay, true); 

    public void OnMove(InputAction.CallbackContext context) 
    {
        if (context.performed) 
        {
            OnMoveEvent.Invoke(context.ReadValue<Vector2>());
        }
        if (context.canceled) 
        {
            OnStopMoveEvent.Invoke();
        }
    }

    public void OnFire(InputAction.CallbackContext context)
    {
        if (context.performed) 
        {
            OnFireEvent.Invoke();
        }
        if (context.canceled) 
        {
            OnStopFireEvent.Invoke();
        }
    }
}
