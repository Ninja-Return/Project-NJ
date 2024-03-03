using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class DayLight : MonoBehaviour
{

    [SerializeField] private Light lightTrm;

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
            SetLightClientRPC(1);
            DayManager.instance.DayCommingClientRPC(false);

        }

        if(angle >= 160 && isLight && angle <= 170)
        {


            isLight = false;
            SetLightClientRPC(0);
            DayManager.instance.NightCommingClientRPC(false);

        }

    }

    [ClientRpc]
    private void SetLightClientRPC(float targetIntansity)
    {

        StartCoroutine(SetLight(targetIntansity));

    }

    private IEnumerator SetLight(float targetIntansity)
    {

        float per = 0;

        float origin = lightTrm.intensity;

        while(per <= 1)
        {

            lightTrm.intensity = Mathf.Lerp(origin, targetIntansity, per);
            per += Time.deltaTime;
            yield return null;

        }

        lightTrm.intensity = targetIntansity;

    }

}
