using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProfileLayout : MonoBehaviour
{
    public float semiMajorAxis; // Ÿ���� ���� ������
    public float semiMinorAxis; // Ÿ���� ���� ������

    int childCount;

    private void Update()
    {
        if (childCount != transform.childCount)
        {
            childCount = transform.childCount;
            ArrangeChildren();
        }
    }

    private void ArrangeChildren() //���� �߰� �� ȣ��
    {
        float angleStep = 360f / childCount;

        for (int i = 0; i < childCount; i++)
        {
            float angle = i * angleStep + 90f;
            float x = transform.position.x + semiMajorAxis * Mathf.Cos(angle * Mathf.Deg2Rad);
            float y = transform.position.y + semiMinorAxis * Mathf.Sin(angle * Mathf.Deg2Rad);

            Transform child = transform.GetChild(i);
            child.position = new Vector3(x, y, 0);
        }
    }
}
