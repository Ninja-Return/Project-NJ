using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Video;
using YoutubePlayer.Components;

public class IntroVideoPlayer : MonoBehaviour
{

    [SerializeField] private VideoPlayer player;
    [SerializeField] private UnityEvent endEvt;
    [SerializeField] private float startDelay;

    private void Start()
    {

        StartCoroutine(StartVideoDelay());

        player.loopPointReached += HandleEnd;

    }

    private void HandleEnd(VideoPlayer source)
    {

        Debug.Log("³¡");
        endEvt?.Invoke();

    }

    private IEnumerator StartVideoDelay()
    {
        yield return new WaitForSeconds(startDelay);

        player.Play();
    }

}
