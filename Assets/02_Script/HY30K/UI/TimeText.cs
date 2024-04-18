using TMPro;
using UnityEngine;

public class TimeText : MonoBehaviour
{

    [SerializeField] private ClearTimeManager clearTimeManager;
    private TMP_Text text;

    private void Awake()
    {

        text = GetComponent<TMP_Text>();

    }

    private void Start()
    {

        HandleTimeChanged(0, clearTimeManager.playerTime.Value);
        clearTimeManager.playerTime.OnValueChanged += HandleTimeChanged;

    }

    private void HandleTimeChanged(int oldTime, int time)
    {

        text.text = $"Time: {time}";

    }

    private void OnDestroy()
    {

        clearTimeManager.playerTime.OnValueChanged -= HandleTimeChanged;

    }

}
