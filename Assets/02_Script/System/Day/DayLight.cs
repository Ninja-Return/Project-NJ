using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class DayLight : MonoBehaviour
{

    [SerializeField] private Transform lightTrm;

    private bool isLight = false;
    private float angle;

    private void Start()
    {

        if (!NetworkManager.Singleton.IsServer) return;

        DayManager.instance.OnTimeUpdate += HandleTimeUpdate;

    }

    private void HandleTimeUpdate(float time)
    {

        angle += (Time.deltaTime / DayManager.instance.timeSpeed) * 180f;

        if (angle > 360) angle = 0;

        lightTrm.transform.eulerAngles = new Vector3(angle, 50, 0);

        if(angle >= 30 && !isLight && angle <= 40)
        {

            isLight = true;
            DayManager.instance.DayCommingClientRPC(false);

        }

        if(angle >= 160 && isLight && angle <= 170)
        {


            isLight = false;
            DayManager.instance.NightCommingClientRPC(false);

        }

    }


    

}
