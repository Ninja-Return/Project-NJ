using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour
{
    [SerializeField] private TMP_Text[] text;
    public float time = 0;

    private void Update()
    {
        if (ClearTimeManager.Instance.TimerStarted)
            time += Time.deltaTime;


        text[0].text = ((int)time / 60 % 60).ToString() + " Ка";
        text[1].text = ((int)time % 60).ToString() + " УЪ";
    }
}
