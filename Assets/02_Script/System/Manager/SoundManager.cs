using System.Collections;
using System.Collections.Generic;
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

public class SoundManager : MonoBehaviour
{

    private static SoundManager instance;
    private static AudioMixer mainMixer;

    private AudioMixerGroup bgmMixer;
    private AudioMixerGroup sfxMixer;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void Init()
    {

        mainMixer = Resources.Load<AudioMixer>("Audio/MainMixer");
        GameObject obj = new GameObject("SoundManager");
        instance = obj.AddComponent<SoundManager>();
        instance.InitInstance();

    }

    private void InitInstance()
    {

        sfxMixer = mainMixer.FindMatchingGroups("SFX")[0];
        bgmMixer = mainMixer.FindMatchingGroups("BGM")[0];

        DontDestroyOnLoad(gameObject);

    }

    public static void Play2DSound(string clipName, SoundType type = SoundType.SFX)
    {

        if (instance == null) return;

        instance.Play2DSoundServerRPC(clipName, type);

    }

    [ServerRpc(RequireOwnership = false)]
    private void Play2DSoundServerRPC(string clipName, SoundType type)
    {

        Play2DSoundClientRPC(clipName, type);

    }

    [ClientRpc]
    private void Play2DSoundClientRPC(string clipName, SoundType type)
    {

        GameObject obj = new GameObject();
        var source = obj.AddComponent<AudioSource>();

        AudioClip clip = Resources.Load<AudioClip>($"Sound/{clipName}");

        source.clip = clip;
        source.spatialBlend = 0;
        source.outputAudioMixerGroup = type switch
        {

            SoundType.SFX => instance.sfxMixer,
            SoundType.BGM => instance.bgmMixer,
            _ => null

        };


        if (type == SoundType.BGM)
        {

            source.loop = true;

        }
        else
        {

            StartCoroutine(SFXDestroyCo(clip.length, obj));

        }

        source.Play();

    }

    [ServerRpc(RequireOwnership = false)]
    private void Play3DSoundServerRPC(string clipName, Vector3 position,
        float minDistance = 1, float maxDistance = 500,
        SoundType type = SoundType.SFX,
        AudioRolloffMode rolloffMode = AudioRolloffMode.Logarithmic)
    {

        Play3DSoundClientRPC(clipName, position, minDistance, maxDistance, type, rolloffMode);

    }

    [ClientRpc]
    private void Play3DSoundClientRPC(string clipName, Vector3 position,
        float minDistance = 1, float maxDistance = 500,
        SoundType type = SoundType.SFX,
        AudioRolloffMode rolloffMode = AudioRolloffMode.Logarithmic)
    {

        GameObject obj = new GameObject();
        obj.transform.position = position;
        var source = obj.AddComponent<AudioSource>();

        AudioClip clip = Resources.Load<AudioClip>($"Sound/{clipName}");

        source.clip = clip;
        source.spatialBlend = 1;
        source.minDistance = minDistance;
        source.maxDistance = maxDistance;
        source.rolloffMode = rolloffMode;

        source.outputAudioMixerGroup = type switch
        {

            SoundType.SFX => instance.sfxMixer,
            SoundType.BGM => instance.bgmMixer,
            _ => null

        };

        if (type == SoundType.BGM)
        {

            source.loop = true;

        }
        else
        {

            StartCoroutine(SFXDestroyCo(clip.length, obj));

        }

        source.Play();

    }

    public static void Play3DSound(string clipName, Vector3 position, 
        float minDistance = 1, float maxDistance = 500, 
        SoundType type = SoundType.SFX, 
        AudioRolloffMode rolloffMode = AudioRolloffMode.Logarithmic)
    {

        if (instance == null) return;

        instance.Play3DSoundServerRPC(clipName, position, minDistance, maxDistance, type, rolloffMode);

    }

    private static IEnumerator SFXDestroyCo(float lenght, GameObject obj)
    {

        yield return new WaitForSeconds(lenght + 0.1f);

        if(obj != null)
        {

            Destroy(obj);

        }

    }

}