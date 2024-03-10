using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Rendering;
using UnityEngine.UIElements;

public enum SoundType
{

    SFX,
    BGM

}

public enum SoundPlayType
{

    Server,
    Local

}

public class NetworkSoundManager : NetworkBehaviour
{

    private static NetworkSoundManager instance;

    private void Awake()
    {

        instance = this;

    }

    public static void Play2DSound(string clipName, SoundType type = SoundType.SFX, SoundPlayType playType = SoundPlayType.Server)
    {

        if (instance == null) return;

        if(playType == SoundPlayType.Server)
        {

            instance.Play2DSoundServerRPC(clipName, type);

        }
        else
        {

            SoundManager.Play2DSound(clipName, type);

        }


    }

    [ServerRpc(RequireOwnership = false)]
    private void Play2DSoundServerRPC(FixedString128Bytes clipName, SoundType type)
    {

        Play2DSoundClientRPC(clipName, type);

    }

    [ClientRpc]
    private void Play2DSoundClientRPC(FixedString128Bytes clipName, SoundType type)
    {

        SoundManager.Play2DSound(clipName.ToString(), type);

    }

    [ServerRpc(RequireOwnership = false)]
    private void Play3DSoundServerRPC(FixedString128Bytes clipName, Vector3 position,
        float minDistance = 1, float maxDistance = 500,
        SoundType type = SoundType.SFX,
        AudioRolloffMode rolloffMode = AudioRolloffMode.Logarithmic)
    {

        Play3DSoundClientRPC(clipName, position, minDistance, maxDistance, type, rolloffMode);

    }

    [ClientRpc]
    private void Play3DSoundClientRPC(FixedString128Bytes clipName, Vector3 position,
        float minDistance = 1, float maxDistance = 500,
        SoundType type = SoundType.SFX,
        AudioRolloffMode rolloffMode = AudioRolloffMode.Logarithmic)
    {

        SoundManager.Play3DSound(clipName.ToString(), position, minDistance, maxDistance, type, rolloffMode);

    }

    public static void Play3DSound(string clipName, Vector3 position, 
        float minDistance = 1, float maxDistance = 500, 
        SoundType type = SoundType.SFX, 
        AudioRolloffMode rolloffMode = AudioRolloffMode.Logarithmic, 
        SoundPlayType playType = SoundPlayType.Server)
    {

        if (instance == null) return;

        if(playType == SoundPlayType.Server)
        {

            instance.Play3DSoundServerRPC(clipName, position, minDistance, maxDistance, type, rolloffMode);

        }
        else
        {

            SoundManager.Play3DSound(clipName, position, minDistance , maxDistance, type, rolloffMode);

        }


    }

}