using Cinemachine;
using System.Collections;
using UnityEngine;

public class HeadBobController : MonoBehaviour
{
    [SerializeField] private float bobFrequency = 5f; // ������ �ֱ�
    [SerializeField] private float bobAmount = 0.1f; // ������ ũ��
    [SerializeField] private float bobAmplitude = 0.1f; // ������ ũ��
    [SerializeField] private float tiltAngle = 8f; // �¿�� ����̴� ����
    private CinemachineVirtualCamera virtualCamera;
    private CinemachineBasicMultiChannelPerlin noise;

    private float timer = 0f;

    void Update()
    {
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
            transform.localPosition = new Vector3(transform.localPosition.x, waveSlice, transform.localPosition.z);

            timer += Time.deltaTime;
            if (timer > Mathf.PI * 2) timer = timer - (Mathf.PI * 2);
        }
        else
        {
            // ������ �� ī�޶� �ʱ� ��ġ�� �ǵ���
            timer = 0;
            transform.localPosition = new Vector3(transform.localPosition.x, 0f, transform.localPosition.z);
        }
        #endregion
    }
}
