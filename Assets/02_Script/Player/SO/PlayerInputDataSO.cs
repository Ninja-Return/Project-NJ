using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(menuName = "SO/Player/Data/Input")]
public class PlayerInputDataSO : ScriptableObject, PlayerInput.IPlayerMovementActions, IDisposable
{

    public event Action OnJumpKeyPress;
    public event Action OnInteractionKeyPress;
    public event Action OnObjectMoveKeyPress;
    public event Action OnObjectMoveKeyUp;
    public event Action OnInventoryActivePress;
    public event Action OnUseObjectKeyPress;
    public event Action OnUseObjectKeyUp;
    public event Action OnSitDownKeyPress;
    public event Action OnSitDownKeyUp;
    public event Action<int> OnInventoryKeyPress;

    private PlayerInput playerInput;

    public Vector2 MoveVecter { get; private set; }
    public Vector2 MouseDelta { get; private set; }

    public PlayerInputDataSO Init()
    {

        var data = Instantiate(this);
        data.Enable();

        return data;

    }

    public void Enable()
    {

        if(playerInput == null)
        {

            playerInput = new PlayerInput();

        }

        playerInput.PlayerMovement.SetCallbacks(this);

        playerInput.PlayerMovement.Enable();

    }

    public void Disable()
    {

        if (playerInput == null) return;

        playerInput.PlayerMovement.SetCallbacks(null);
        playerInput.PlayerMovement.Disable();

    }

    public void OnCamera(InputAction.CallbackContext context)
    {

        MouseDelta = context.ReadValue<Vector2>();

    }

    public void OnJump(InputAction.CallbackContext context)
    {

        if (context.performed)
        {

            OnJumpKeyPress?.Invoke();

        }

    }

    public void OnMove(InputAction.CallbackContext context)
    {

        MoveVecter = context.ReadValue<Vector2>();

    }

    public void OnInteraction(InputAction.CallbackContext context)
    {

        if (context.performed)
        {

            OnInteractionKeyPress?.Invoke();

        }

    }

    public void OnSitDown(InputAction.CallbackContext context)
    {

        if (context.performed)
        {

            OnSitDownKeyPress?.Invoke();

        }else if (context.canceled)
        {
            OnSitDownKeyUp?.Invoke();
        }

    }

    public void OnMoveObject(InputAction.CallbackContext context)
    {

        if (context.performed)
        {

            OnObjectMoveKeyPress?.Invoke();

        }
        else if (context.canceled)
        {
            OnObjectMoveKeyUp?.Invoke();
        }


    }

    public void OnInventoryActive(InputAction.CallbackContext context)
    {

        if (context.performed)
        {

            OnInventoryActivePress?.Invoke();

        }

    }

    public void OnUseHandObject(InputAction.CallbackContext context)
    {

        if (context.performed)
        {

            OnUseObjectKeyPress?.Invoke();

        }
        else if (context.canceled)
        {

            OnUseObjectKeyUp?.Invoke();

        }

    }

    public void OnInventoryKey(InputAction.CallbackContext context)
    {
        var key = int.Parse(context.control.name);

        OnInventoryKeyPress?.Invoke(key);
    }

    public void Dispose()
    {

        playerInput.Dispose();

    }

    public void InitMoveVector()
    {

        MoveVecter = Vector3.zero;

    }

}
