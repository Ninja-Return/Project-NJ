using Cinemachine;
using DG.Tweening;
using System.Collections;
using Unity.Netcode;
using UnityEngine;

public class StoryModeManager : MonoBehaviour
{

    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private GameObject door;
    [SerializeField] private AudioSource startSource;
    [SerializeField] private GameObject[] decals;
    [SerializeField] private CinemachineImpulseSource sheckSource;
    [Header("ºñ¸í&ÃÑ ºÎºÐ")]
    [SerializeField] private AudioSource screamSound, gunShootSound, turnOffSound;
    [SerializeField] private GameObject gunlightObj;
    [SerializeField] private TV tv;
    [SerializeField] private NetworkObject monster;
    [SerializeField] private Transform mosterSpawnPos;

    private void Start()
    {

        PlayerManager.Instance.SpawnPlayer(0);
        StartCoroutine(LogicCo());

    }


    private IEnumerator LogicCo()
    {

        yield return new WaitForSeconds(3f);

        startSource.Pause();
        turnOffSound.Play();
        sheckSource.GenerateImpulse();

        yield return new WaitForSeconds(2f);
        StartCoroutine(BlinkLight(false));
        yield return new WaitForSeconds(1f);

        yield return StartCoroutine(ScreamCo());
        yield return new WaitForSeconds(1);
        yield return StartCoroutine(GunShootCo());
        yield return StartCoroutine(DecalsEnableCo());
        yield return new WaitForSeconds(1);
        yield return StartCoroutine(RedLightCo());
        yield return new WaitForSeconds(1);
        tv.On();
        door.transform.DOMoveY(-3, 0.5f);
        var obj = Instantiate(monster, mosterSpawnPos.position, Quaternion.identity);
        obj.Spawn(true);

    }

    private IEnumerator RedLightCo()
    {

        MapLightSystem.Instance.SetLightColor(Color.red);
        yield return StartCoroutine(BlinkLight(true));

    }

    private IEnumerator GunLightEft()
    {

        while (true)
        {

            yield return new WaitForSeconds(0.05f);
            gunlightObj.SetActive(!gunlightObj.activeSelf);

        }
        

    }

    private IEnumerator GunShootCo()
    {

        var co = StartCoroutine(GunLightEft());

        gunShootSound.Play();
        yield return new WaitUntil(() => !gunShootSound.isPlaying);
        StopCoroutine(co);
        gunlightObj.SetActive(false);

    }

    private IEnumerator ScreamCo()
    {

        screamSound.Play();
        yield return new WaitForSeconds(1.5f);

    }

    private IEnumerator DecalsEnableCo()
    {

        for(int i = 0; i < 4; i++)
        {

            decals[i].SetActive(true);
            turnOffSound.Play();

            StartCoroutine(BlinkLight(true));
            sheckSource.GenerateImpulse();

            yield return new WaitForSeconds(1f);

            StartCoroutine(BlinkLight(false));

            yield return new WaitForSeconds(2f);
        }

    }

    private IEnumerator BlinkLight(bool enable)
    {

        for(int i = 0; i < 5; i++)
        {
            MapLightSystem.Instance.SetLightEnable(false);
            yield return new WaitForSeconds(0.01f);
            MapLightSystem.Instance.SetLightEnable(true);
            yield return new WaitForSeconds(0.01f);
        }

        MapLightSystem.Instance.SetLightEnable(enable);

    }

}
