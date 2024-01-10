using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputDataSO : ScriptableObject, PlayerInput.IPlayerMovementActions
{

    public PlayerInputDataSO Init()
    {

        var data = Instantiate(this);

    }

    public void OnCamera(InputAction.CallbackContext context)
    {



    }

    public void OnJump(InputAction.CallbackContext context)
    {



    }

    public void OnMove(InputAction.CallbackContext context)
    {



    }

}
