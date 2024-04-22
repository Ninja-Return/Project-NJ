using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DumbellTrigger : MonoBehaviour
{
    private PlayerController controller;

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log("¥Í¿Ω");
            controller = collision.GetComponent<PlayerController>();
            Debug.Log(controller);
            if(controller.Data.MoveSpeed.Value > 3f)
            {
                controller.AddSpeed(-2f, 10f);
            }
            Destroy(this.gameObject);
        }
    }
}
