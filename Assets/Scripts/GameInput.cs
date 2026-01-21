using System;
using UnityEngine;

public class GameInput : MonoBehaviour
{
    public event EventHandler OnInteractAction;
    private PlayerInputActions inputActions;

    private void Awake()
    {
        inputActions = new PlayerInputActions();
        inputActions.Player.Enable();

        inputActions.Player.Interact.performed += Interact_performed;
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
}
