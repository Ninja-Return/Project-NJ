using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestItem : MonoBehaviour
{
    [SerializeField] private SlotData slotData;

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("���� �Դ´�");
        Destroy(gameObject);
    }
}
