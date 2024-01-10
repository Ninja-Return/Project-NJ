using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(menuName = "SO/Player/Data/Input")]
public class PlayerInputDataSO : ScriptableObject, PlayerInput.IPlayerMovementActions
{

    public event Action OnJumpKeyPress;

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

        playerInput = new PlayerInput();
        playerInput.PlayerMovement.SetCallbacks(this);

        playerInput.PlayerMovement.Enable();

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

}
