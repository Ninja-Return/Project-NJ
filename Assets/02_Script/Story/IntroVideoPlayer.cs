using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Video;
using YoutubePlayer.Components;

public class IntroVideoPlayer : MonoBehaviour
{

    [SerializeField] private VideoPlayer player;
    [SerializeField] private UnityEvent endEvt;

    private void Start()
    {
        
        player.loopPointReached += HandleEnd;

    }

    private void HandleEnd(VideoPlayer source)
    {

        Debug.Log("³¡");
        endEvt?.Invoke();

    }

}
