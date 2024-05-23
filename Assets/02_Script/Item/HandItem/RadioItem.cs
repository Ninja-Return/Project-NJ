using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadioItem : HandItemRoot
{
    private AudioSource audioSource;

    private bool isPlay;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public override void DoUse()
    {
        isPlay = !isPlay;

        if (isPlay)
            audioSource.Play();
        else
            audioSource.Stop();
    }
}
