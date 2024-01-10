using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCameraRotate : PlayerStateRoot
{

    private Transform cameraTrm;
    private Vector3 angle;

    public PlayerCameraRotate(PlayerController controller) : base(controller)
    {

        cameraTrm = transform.Find("PlayerCamera");
        angle = cameraTrm.localEulerAngles;

    }

    protected override void EnterState()
    {

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

    }

    protected override void UpdateState()
    {

        CameraRotate();
        TrmRotate();

    }

    private void CameraRotate()
    {
        ///
        var vec = new Vector3(-input.MouseDelta.y, input.MouseDelta.x);

        angle += vec * data.LookSensitive.Value * Time.deltaTime;

        angle.x = Mathf.Clamp(angle.x, -60, 60);

        cameraTrm.eulerAngles = angle;

    }

    private void TrmRotate()
    {

        var rot = cameraTrm.rotation;
        rot.x = 0;
        rot.z = 0;

        transform.rotation = rot;

    }

    protected override void ExitState()
    {

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

    }

}