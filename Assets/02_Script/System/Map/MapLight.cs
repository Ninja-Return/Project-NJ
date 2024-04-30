using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapLight : MonoBehaviour
{

    private Light curLight;

    private void Start()
    {

        curLight = GetComponent<Light>();

        MapLightSystem.Instance.AddLight(this);

    }

    public void SetLight(bool value)
    {

        StartCoroutine(BlinkCo(value));

    }

    private IEnumerator BlinkCo(bool value)
    {

        for(int i = 1; i < 4; i++)
        {

            yield return new WaitForSeconds(0.3f / i);
            curLight.enabled = !curLight.enabled;

        }

        curLight.enabled = value;

    }


}