using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

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

    }

    public static void Play2DSound(AudioClip clip, float volume, SoundType type = SoundType.SFX)
    {

        if (instance == null) return;

        GameObject obj = new GameObject();
        var source = obj.AddComponent<AudioSource>();

        source.clip = clip;
        source.volume = volume;
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

            instance.StartCoroutine(SFXDestroyCo(clip.length, obj));

        }

        source.Play();

    }

    public static void Play3DSound(AudioClip clip, float volume, Vector3 position, 
        float minDistance = 1, float maxDistance = 500, 
        SoundType type = SoundType.SFX, 
        AudioRolloffMode rolloffMode = AudioRolloffMode.Logarithmic)
    {

        if (instance == null) return;

        GameObject obj = new GameObject();
        obj.transform.position = position;
        var source = obj.AddComponent<AudioSource>();

        source.clip = clip;
        source.volume = volume;
        source.spatialBlend = 1;
        source.minDistance = minDistance;
        source.maxDistance = maxDistance;
        source.rolloffMode = rolloffMode;



        if (type == SoundType.BGM)
        {

            source.loop = true;

        }
        else
        {

            instance.StartCoroutine(SFXDestroyCo(clip.length, obj));

        }

        source.Play();

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