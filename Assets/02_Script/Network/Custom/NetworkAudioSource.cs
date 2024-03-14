using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;


[RequireComponent(typeof(AudioSource))]
public class NetworkAudioSource : NetworkBehaviour
{

    private AudioSource source;
    public bool isPlaying => source.isPlaying;
    
    private enum NetworkAudioType
    {

        Play,
        Stop, 
        Pause

    }

    private void Awake()
    {
        
        source = GetComponent<AudioSource>();

    }

    private void Start()
    {

        if (source.playOnAwake)
        {

            EventClientRPC(NetworkAudioType.Play);

        }

    }

    public void Play()
    {

        EventServerRPC(NetworkAudioType.Play);

    }

    public void Pause()
    {

        EventServerRPC(NetworkAudioType.Pause);

    }

    public void Stop()
    {

        EventServerRPC(NetworkAudioType.Stop);

    }

    [ServerRpc(RequireOwnership = false)]
    private void EventServerRPC(NetworkAudioType type)
    {

        EventClientRPC(type);

    }

    [ClientRpc]
    private void EventClientRPC(NetworkAudioType type)
    {

        switch (type)
        {
            case NetworkAudioType.Play:
                source.Play();
                break;
            case NetworkAudioType.Stop:
                source.Stop();
                break;
            case NetworkAudioType.Pause:
                source.Pause();
                break;
        }

    }

}
