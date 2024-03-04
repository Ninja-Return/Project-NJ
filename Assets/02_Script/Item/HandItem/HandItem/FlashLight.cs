using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashLight : HandItemRoot
{

    [SerializeField] private GameObject flashlight;

    public override void DoUse()
    {

        flashlight.SetActive(!flashlight.activeSelf);

    }

}
