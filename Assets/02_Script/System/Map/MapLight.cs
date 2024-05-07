using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class MapLight : NetworkBehaviour
{

    private Light curLight;

    public override void OnNetworkSpawn()
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

        for(int i = 1; i < 10; i++)
        {

            yield return new WaitForSeconds(0.5f / i);
            curLight.enabled = !curLight.enabled;

        }

        curLight.enabled = value;

    }


}