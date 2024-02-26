using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class DayManager : NetworkBehaviour
{

    [field:SerializeField] public float timeSpeed;

    private float time;
    private bool timeStop = false;

    public event Action<float> OnTimeUpdate;
    public event Action OnDayComming;
    public event Action OnNightComming;

    public static DayManager instance;

    private void Awake()
    {
        
        instance = this;

    }

    private void Start()
    {

        GameManager.Instance.OnGameStarted += HandleStartedGame;

    }

    private void HandleStartedGame()
    {

        if (IsServer)
        {

            StartCoroutine(DayCo());

        }

    }

    private void HandleTimeValueChanged(float newValue)
    {

        OnTimeUpdate?.Invoke(newValue);

    }

    private IEnumerator DayCo()
    {

        yield return new WaitForSeconds(10f);
        var wait = new WaitUntil(() => !timeStop);

        while (true)
        {

            yield return wait;
            time += Time.deltaTime / timeSpeed;
            HandleTimeValueChanged(time);
            yield return null;

        }

    }

    public override void OnDestroy()
    {

        base.OnDestroy();

        instance = null;

    }

    [ClientRpc]
    public void DayCommingClientRPC(bool timeStop)
    {

        TimeSetting(timeStop);
        OnDayComming?.Invoke();

    }

    [ClientRpc]
    public void NightCommingClientRPC(bool timeStop)
    {

        TimeSetting(timeStop);
        OnNightComming?.Invoke();

    }

    public void TimeSetting(bool isStop)
    {

        timeStop = isStop;

    }

}
