using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class DayVolume : MonoBehaviour
{

    [SerializeField] private Volume targetVolume;

    private void Start()
    {

        DayManager.instance.OnDayComming += HandleDayComming;
        DayManager.instance.OnNightComming += HandleNightComming;

    }

    private void HandleDayComming()
    {

        StartCoroutine(SetVolume(0));

    }

    private void HandleNightComming()
    {

        StartCoroutine(SetVolume(1)); 

    }


    private IEnumerator SetVolume(float target)
    {

        float per = 0;

        float origin = targetVolume.weight;

        while (per <= 1)
        {

            targetVolume.weight = Mathf.Lerp(origin, target, per);
            per += Time.deltaTime;
            yield return null;

        }

        targetVolume.weight = target;

    }

}
