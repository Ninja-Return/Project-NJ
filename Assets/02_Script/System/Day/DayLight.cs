using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayLight : MonoBehaviour
{

    [SerializeField] private Light lightTrm;

    private bool isLight = true;
    private float angle;

    private void Start()
    {

        DayManager.instance.OnTimeUpdate += HandleTimeUpdate;

    }

    private void HandleTimeUpdate(float time)
    {

        angle += (Time.deltaTime / DayManager.instance.timeSpeed) * 180f;

        if (angle > 360) angle = 0;

        lightTrm.transform.eulerAngles = new Vector3(angle, 50, 0);

        Debug.Log(angle);

        if(angle >= 30 && !isLight && angle <= 40)
        {

            isLight = true;
            StartCoroutine(SetLight(1));
            DayManager.instance.DayComming(false);

        }

        if(angle >= 160 && isLight && angle <= 170)
        {


            isLight = false;
            StartCoroutine(SetLight(0));
            DayManager.instance.NightComming(false);

        }

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
