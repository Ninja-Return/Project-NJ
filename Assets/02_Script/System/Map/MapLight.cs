using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class MapLight : NetworkBehaviour
{

    private Light curLight;

    public IEnumerator Start()
    {

        yield return new WaitUntil(() => {

            return MapLightSystem.Instance != null;
        });

        curLight = GetComponentInChildren<Light>();

        MapLightSystem.Instance.AddLight(this);


    }

    public void SetLight(bool value)
    {

        StartCoroutine(BlinkCo(value));

    }

    private IEnumerator BlinkCo(bool value)
    {

        for(int i = 1; i < 10; i++)
        {

            yield return new WaitForSeconds(0.5f / i);
            curLight.enabled = !curLight.enabled;

        }

        curLight.enabled = value;

    }

    public void Enable(bool value)
    {

        curLight.enabled = value;

    }

    public void SetColor(Color color)
    {

        curLight.color = color;

    }

}