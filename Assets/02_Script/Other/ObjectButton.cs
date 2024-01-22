using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ObjectButton : MonoBehaviour
{
    public UnityEvent buttonClick;

    Camera cam;

    private void Awake()
    {
        cam = Camera.main;
    }

    //Input시스템에 연결
    public void PressButton()
    {
        Ray ray = cam.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 100))
        {
            if (hit.collider.gameObject == gameObject)
            {
                buttonClick?.Invoke();
            }
        }
    }
}
