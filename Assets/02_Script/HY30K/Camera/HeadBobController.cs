using Cinemachine;
using UnityEngine;

public class HeadBobController : MonoBehaviour
{
    public float bobbingSpeed = 0.18f;
    public float bobbingAmount = 0.2f;
    public float strideLengthen = 0.3f;
    public float tiltAngle = 15f;

    private float originalY;
    private float timer = 0.0f;

    void Start()
    {
        originalY = transform.localPosition.y;
    }

    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        Vector3 movement = new Vector3(horizontal, 0, vertical);

        if (movement.magnitude > 0.01f)
        {
            float waveSlice = Mathf.Sin(timer);
            float bobbingAmountToUse = bobbingAmount;

            if (waveSlice != 0)
            {
                float translateChange = waveSlice * bobbingAmountToUse;
                transform.localPosition = new Vector3(transform.localPosition.x, originalY + translateChange, transform.localPosition.z);

                // 좌우로 기울이기 효과 추가
                float tilt = Mathf.Sin(timer * 2) * tiltAngle;
                transform.localRotation = Quaternion.Euler(0, 0, tilt);
            }
            else
            {
                transform.localPosition = new Vector3(transform.localPosition.x, originalY, transform.localPosition.z);
                transform.localRotation = Quaternion.identity;
            }

            timer += bobbingSpeed * Time.deltaTime * Mathf.Clamp(movement.magnitude * strideLengthen, 1f, 2f);

            if (timer > Mathf.PI * 2)
            {
                timer = timer - (Mathf.PI * 2);
            }
        }
        else
        {
            timer = 0.0f;
            transform.localPosition = new Vector3(transform.localPosition.x, originalY, transform.localPosition.z);
            transform.localRotation = Quaternion.identity;
        }
    }
}
