using Cinemachine;
using System;
using UnityEngine;

public class PlayerSitDown : PlayerStateRoot
{
    private Transform playerCameraTransform;

    private Vector3 sitDownOffset = new Vector3(0f, -40f, 0f); 
    private float moveSpeed = 100f; 

    private Vector3 originalCameraPosition; 
    private Vector3 targetCameraPosition; 
    private PlayerController controllers;

    public PlayerSitDown(PlayerController controller) : base(controller)
    {
        playerCameraTransform = controller.cvcam.transform;
    }

    protected override void EnterState()
    {
        controllers = transform.root.GetComponent<PlayerController>();
        originalCameraPosition = playerCameraTransform.localPosition;
        targetCameraPosition = originalCameraPosition + sitDownOffset;
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
            Debug.Log("앉음");
        }
        else if (Vector3.Distance(playerCameraTransform.localPosition, targetCameraPosition) < 0.01f)
        {
            controllers.isSittingDown = false;
            playerCameraTransform.localPosition = originalCameraPosition;
            Debug.Log("돌아감");
        }
        else
        {
            // 내리기 애니메이션 해
            playerCameraTransform.localPosition = Vector3.MoveTowards(playerCameraTransform.localPosition, targetCameraPosition, moveSpeed * Time.deltaTime);
            Debug.Log(playerCameraTransform.localPosition);
            Debug.Log(targetCameraPosition);
            Debug.Log(Vector3.Distance(playerCameraTransform.localPosition, targetCameraPosition));
        }
    }
}
