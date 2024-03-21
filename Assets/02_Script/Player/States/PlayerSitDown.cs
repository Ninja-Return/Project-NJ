using Cinemachine;
using System;
using UnityEngine;
using DG.Tweening;

public class PlayerSitDown : PlayerStateRoot
{
    private Transform playerCameraTransform;

    private Vector3 sitDownOffset = new Vector3(0f, -0.3f, 0f); 
    
   
    
    private PlayerController controllers;

    public PlayerSitDown(PlayerController controller) : base(controller)
    {
        playerCameraTransform = controller.cvcam.transform;
    }

    protected override void EnterState()
    {
        controllers = transform.root.GetComponent<PlayerController>();
        controllers.targetCameraPosition = controllers.originalCameraPosition + sitDownOffset;
        input.OnSitDownKeyPress += HandleSitDownKeyPress;
    }
    


    protected override void ExitState()
    {
        input.OnSitDownKeyPress -= HandleSitDownKeyPress;
    }

    private void HandleSitDownKeyPress()
    {
        if (!controllers.isSittingDown)
        {
            controllers.isSittingDown = true;
            Debug.Log("æ…¿Ω");
        }
        else if (controllers.isSittingDown && Input.GetKeyDown(KeyCode.LeftControl))
        {
            controllers.isSittingDown = false;
            playerCameraTransform.DOLocalMoveY(controllers.originalCameraPosition.y, controllers.changeTime);
            Debug.Log("µπæ∆∞®");
        }
        
    }
}

