using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapLight : MonoBehaviour
{

    private Light curLight;

    private void Awake()
    {

        curLight = GetComponent<Light>();

        MapLightSystem.Instance.AddLight(this);

    }

    public void SetLight(bool value)
    {

        curLight.enabled = value;

    }

}