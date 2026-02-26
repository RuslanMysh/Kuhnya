using System;
using UnityEngine;

public class GameInput : MonoBehaviour
{
    public static GameInput Instance { get; private set; }

    public event EventHandler OnInteractAction;
    public event EventHandler OnInteractAlternateAction;
    public event EventHandler OnDash;
    public event EventHandler OnPauseAction;
    private PlayerInputActions inputActions;

    private void Awake()
    {
        inputActions = new PlayerInputActions();
        inputActions.Player.Enable();

        inputActions.Player.Interact.performed += Interact_performed;
        inputActions.Player.InteractAlternate.performed += InteractAlternate_performed;
        inputActions.Player.Dash.started += Dash_started;
        inputActions.Player.Pause.performed += Pause_performed;

        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.Log("Больше одного GameInput!");
        }

    }

    private void OnDestroy()
    {
        inputActions.Player.Interact.performed -= Interact_performed;
        inputActions.Player.InteractAlternate.performed -= InteractAlternate_performed;
        inputActions.Player.Dash.started -= Dash_started;
        inputActions.Player.Pause.performed -= Pause_performed;

        inputActions.Dispose();
    }

    private void Pause_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnPauseAction?.Invoke(this, EventArgs.Empty);
    }

    private void Dash_started(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnDash?.Invoke(this, EventArgs.Empty);
    }

    private void InteractAlternate_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnInteractAlternateAction?.Invoke(this, EventArgs.Empty);
    }

    private void Interact_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnInteractAction?.Invoke(this, EventArgs.Empty);
    }

    public Vector2 GetMovementNormalized()
    {
        Vector2 inputVector = inputActions.Player.Move.ReadValue<Vector2>();


        inputVector = inputVector.normalized;
        return inputVector;
    }

    public float GetSprint()
    {
         float sprint = inputActions.Player.Sprint.ReadValue<float>();

         return sprint;
    }

    public float GetDash()
    {
        float dash = inputActions.Player.Dash.ReadValue<float>();

        return dash;
    }
}
