using Cinemachine;
using DG.Tweening;
using System.Collections;
using Unity.Netcode;
using UnityEngine;

public class HeadBobController : NetworkBehaviour
{
    [SerializeField] private float bobFrequency = 5f; // ������ �ֱ�
    [SerializeField] private float bobAmount = 0.1f; // ������ ũ��
    [SerializeField] private float bobAmplitude = 0.1f; // ������ ũ��
    [SerializeField] private float tiltAngle = 8f; // �¿�� ����̴� ����
    private PlayerController controller;

    private float timer = 0f;
    private float sittimer = 0f;


    private void Start()
    {
        if (!IsOwner) Destroy(this);
        controller = transform.root.GetComponent<PlayerController>();

    }

    void Update()
    {

        if (controller.CurrentState != EnumPlayerState.Move) return;


        float horizontalMovement = Input.GetAxis("Horizontal");
        float verticalMovement = Input.GetAxis("Vertical");

        float tilt = horizontalMovement * tiltAngle;
       transform.localRotation = Quaternion.Euler(transform.localEulerAngles.x,transform.localEulerAngles.y, -tilt);

        #region 1 (Just Math)
        if (Mathf.Abs(horizontalMovement) > 0.1f && !controller.isSittingDown || Mathf.Abs(verticalMovement) > 0.1f && !controller.isSittingDown)
        {

            sittimer += Time.deltaTime;
            // ī�޶��� ��ġ�� ���Ʒ��� ������
            float waveSlice = Mathf.Sin(timer * bobFrequency) * bobAmount;
            controller.originalCameraPosition = transform.localPosition;
            if (sittimer > 0.6f)
            {
                transform.localPosition = new Vector3(transform.localPosition.x, waveSlice + 0.6f, transform.localPosition.z);
            }


            timer += Time.deltaTime;
            if (timer > Mathf.PI * 2) timer = timer - (Mathf.PI * 2);
        }
        else if (controller.isSittingDown)
        {
            timer = 0;
            sittimer = 0;
            transform.DOLocalMoveY(controller.targetCameraPosition.y, controller.changeTime);
        }

        #endregion
    }
}
