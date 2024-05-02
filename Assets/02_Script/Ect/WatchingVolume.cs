using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class WatchingVolume : MonoBehaviour
{

    private Volume volume;

    private void Awake()
    {

        volume = GetComponent<Volume>();

    }

    private void Start()
    {

        WatchingSystem.Instance.OnWatchingStarted += HandleWatchingStart;

    }

    private void HandleWatchingStart()
    {

        volume.weight = 0.3f;

    }

}
