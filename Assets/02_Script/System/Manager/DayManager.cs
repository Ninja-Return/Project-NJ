using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class DayManager : NetworkBehaviour
{

    [field:SerializeField] public float timeSpeed;

    private NetworkVariable<float> time = new NetworkVariable<float>();
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


        if (IsClient)
        {

            time.OnValueChanged += HandleTimeValueChanged;

        }


        if (IsServer)
        {

            StartCoroutine(DayCo());

        }

    }

    private void HandleTimeValueChanged(float previousValue, float newValue)
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
            time.Value += Time.deltaTime / timeSpeed;
            yield return null;

        }

    }

    public override void OnDestroy()
    {

        base.OnDestroy();

        instance = null;

    }

    public void DayComming(bool timeStop)
    {

        TimeSetting(timeStop);
        OnDayComming?.Invoke();

    }

    public void NightComming(bool timeStop)
    {

        TimeSetting(timeStop);
        OnNightComming?.Invoke();

    }

    public void TimeSetting(bool isStop)
    {

        timeStop = isStop;

    }

}
