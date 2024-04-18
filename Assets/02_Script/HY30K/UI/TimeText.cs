using TMPro;
using UnityEngine;

public class TimeText : MonoBehaviour
{

    [SerializeField] private ClearTimeManager clearTimeManager;
    [SerializeField] private TMP_Text[] text;

    private void Start()
    {

        HandleTimeChanged(0, clearTimeManager.playerTime.Value);
        clearTimeManager.playerTime.OnValueChanged += HandleTimeChanged;

    }

    private void HandleTimeChanged(float oldTime, float time)
    {

        text[0].text = ((int)time / 60 % 60).ToString() + " Ка";
        text[1].text = ((int)time % 60).ToString() + " УЪ";

    }

    private void OnDestroy()
    {

        clearTimeManager.playerTime.OnValueChanged -= HandleTimeChanged;

    }

}
