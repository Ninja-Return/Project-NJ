using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SettingUI : MonoBehaviour
{
    [SerializeField] private RectTransform mapPanel;
    [SerializeField] private Slider sensitivitySlider;
    [SerializeField] private Slider SoundSlider;
    [SerializeField] private PlayerDataSO playerData;
    private bool Setting = false;
    private bool isShow = true;
    private float mouseSensitivity = 100f; 

    void Start()
    {
        sensitivitySlider.maxValue = 12;
        sensitivitySlider.value = 12;
        playerData.LookSensitive.SetValue(sensitivitySlider.value);
        sensitivitySlider.onValueChanged.AddListener(delegate { UpdateSensitivity(); });
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !Setting)
        {
            Support.SettingCursorVisable(isShow);
            mapPanel.DOLocalMove(Vector2.zero, 0.5f).SetEase(Ease.InExpo);
            Setting = true;
        }
        else if (Input.GetKeyDown(KeyCode.Escape) && Setting)
        {
            Support.SettingCursorVisable(!isShow);
            mapPanel.DOLocalMove(new Vector2(0, 1200f), 0.5f).SetEase(Ease.OutExpo);
            Setting = false;
        }
    }

    public void PanelUp()
    {
        if (Setting)
        {
            Support.SettingCursorVisable(!isShow);
            mapPanel.DOLocalMove(new Vector2(0, 1200f), 0.5f).SetEase(Ease.OutExpo);
            Setting = false;
        }
    }

    public void GameExit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    public void GotoMain()
    {
        SceneManager.LoadScene(SceneList.LobbySelectScene);
    }

    public void UpdateSensitivity()
    {
        playerData.LookSensitive.SetValue(sensitivitySlider.value);
    }

    public void SoundChange()
    {
        SoundManager.SettingBgm(SoundSlider.value);
    }
}
