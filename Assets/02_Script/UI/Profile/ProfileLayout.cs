using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProfileLayout : MonoBehaviour
{
    public GameObject centerPoint; // �߽���
    public float semiMajorAxis; // Ÿ���� ���� ������
    public float semiMinorAxis; // Ÿ���� ���� ������

    void Start()
    {
        ArrangeChildren();
    }

    void ArrangeChildren() //������ ������ �̻��� ���������� ȣ��
    {
        int childCount = transform.childCount;
        if (childCount == 0) return;

        float angleStep = 360f / childCount;

        for (int i = 0; i < childCount; i++)
        {
            float angle = i * angleStep + 90f;
            float x = centerPoint.transform.position.x + semiMajorAxis * Mathf.Cos(angle * Mathf.Deg2Rad);
            float y = centerPoint.transform.position.y + semiMinorAxis * Mathf.Sin(angle * Mathf.Deg2Rad);

            Transform child = transform.GetChild(i);
            child.position = new Vector3(x, y, 0f);
        }
    }
}
