using Cinemachine;
using System.Collections;
using Unity.Netcode;
using UnityEngine;

public class HeadBobController : NetworkBehaviour
{
    [SerializeField] private float bobFrequency = 5f; // ������ �ֱ�
    [SerializeField] private float bobAmount = 0.1f; // ������ ũ��
    [SerializeField] private float bobAmplitude = 0.1f; // ������ ũ��
    [SerializeField] private float tiltAngle = 8f; // �¿�� ����̴� ����
    private CinemachineVirtualCamera virtualCamera;
    private CinemachineBasicMultiChannelPerlin noise;
    private PlayerController controller;

    private float timer = 0f;

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

        // �¿�� ������ �� ī�޶� �¿�� �����
        float tilt = horizontalMovement * tiltAngle;
        transform.localRotation = Quaternion.Euler(0f, 0f, -tilt);

        #region 1 (Just Math)
        if (Mathf.Abs(horizontalMovement) > 0.1f || Mathf.Abs(verticalMovement) > 0.1f)
        {
            // ī�޶��� ��ġ�� ���Ʒ��� ������
            float waveSlice = Mathf.Sin(timer * bobFrequency) * bobAmount;
            transform.localPosition = new Vector3(transform.localPosition.x, waveSlice + 0.6f, transform.localPosition.z);

            timer += Time.deltaTime;
            if (timer > Mathf.PI * 2) timer = timer - (Mathf.PI * 2);
        }
        else
        {
            // ������ �� ī�޶� �ʱ� ��ġ�� �ǵ���
            timer = 0;
            transform.localPosition = new Vector3(transform.localPosition.x, 0.6f, transform.localPosition.z);
        }
        #endregion
    }
}
