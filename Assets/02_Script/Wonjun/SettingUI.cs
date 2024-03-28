using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SettingUI : MonoBehaviour
{
    [SerializeField] private RectTransform mapPanel;
    [SerializeField] private RectTransform SoundPanel;
    [SerializeField] private RectTransform DPIPanel;
    [SerializeField] private Slider sensitivitySlider;
    [SerializeField] private PlayerDataSO playerData;
    private float panelTime = 0;
    private bool Setting = false;
    private bool isShow = true;
    private PlayerController playerController;

    void Start()
    {
        playerController = GetComponent<PlayerController>();
        sensitivitySlider.maxValue = 12;
        sensitivitySlider.value = 12;
        playerData.LookSensitive.SetValue(sensitivitySlider.value);
        sensitivitySlider.onValueChanged.AddListener(delegate { UpdateSensitivity(); });
    }

    void Update()
    {
        panelTime += Time.deltaTime;
        if (Input.GetKeyDown(KeyCode.Escape) && !Setting && panelTime > .5f)
        {

            Support.SettingCursorVisable(isShow);

            if (PlayerManager.Instance == null)
                playerController.Active(!isShow);
            else
                PlayerManager.Instance.localController.Active(!isShow);



            Setting = true;
            mapPanel.DOLocalMove(Vector2.zero, 0.5f).SetEase(Ease.InExpo);
            panelTime = 0;
        }
        else if (Input.GetKeyDown(KeyCode.Escape) && Setting && panelTime > .5f)
        {

            Support.SettingCursorVisable(!isShow);

            if (PlayerManager.Instance == null)
                playerController.Active(isShow);
            else
                PlayerManager.Instance.localController.Active(isShow);



            Setting = false;
            mapPanel.DOLocalMove(new Vector2(0, 1200f), 0.5f).SetEase(Ease.OutExpo);
            panelTime = 0;
        }
    }

    public void PanelUp()
    {
        if (Setting && panelTime > .5f)
        {
            Support.SettingCursorVisable(!isShow);

            if (PlayerManager.Instance == null)
                playerController.Active(isShow);
            else
                PlayerManager.Instance.localController.Active(isShow);
            Setting = false;
            mapPanel.DOLocalMove(new Vector2(0, 1200f), 0.5f).SetEase(Ease.OutExpo);
            panelTime = 0;
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

    public void SFXChange(Slider slider)
    {
        SoundManager.SettingSfx(slider.value);
    }

    public void BGMChange(Slider slider)
    {
        SoundManager.SettingBgm(slider.value);
    }
}
