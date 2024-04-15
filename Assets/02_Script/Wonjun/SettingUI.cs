using DG.Tweening;
using Michsky.UI.Dark;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SettingUI : MonoBehaviour
{
    [SerializeField] private Slider sensitivitySlider;
    [SerializeField] private PlayerDataSO playerData;
    public MainPanelManager mainPanelManager;
    public UIDissolveEffect dissolveEffect;

    private FullScreenMode screenMode = FullScreenMode.FullScreenWindow;
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
        if (Input.GetKeyDown(KeyCode.Escape) && !Setting && panelTime > .8f)
        {
            mainPanelManager.OpenPanel("Settings");
            //dissolveEffect.DissolveIn();

            Support.SettingCursorVisable(isShow);

            if (PlayerManager.Instance == null)
                playerController.Active(!isShow);
            else
                PlayerManager.Instance.localController.Active(!isShow);


            Setting = true;
            panelTime = 0;
        }
        else if (Input.GetKeyDown(KeyCode.Escape) && Setting && panelTime > .5f)
        {
            mainPanelManager.OpenFirstTab();
            //dissolveEffect.DissolveOut();

            Support.SettingCursorVisable(!isShow);

            if (PlayerManager.Instance == null)
                playerController.Active(isShow);
            else
                PlayerManager.Instance.localController.Active(isShow);

            Setting = false;
            panelTime = 0;
        }
    }

    /*public void PanelUp()
    {
        if (Setting && panelTime > .5f)
        {
            Support.SettingCursorVisable(!isShow);

            Setting = false;

            if (PlayerManager.Instance == null)
                playerController.Active(isShow);
            else
                PlayerManager.Instance.localController.Active(isShow);

            panelTime = 0;
        }
    }*/

    public void FOVSetting(Slider slider) // 50 ~ 70 (처음 60으로 고정)
    {
        PlayerPrefs.SetFloat("FOV", slider.value);
    }

    public void ResolutionSetting(TMP_Dropdown dropdown)
    {
        int value = dropdown.value;
        var optionText = dropdown.options[value].text;
        var textOut = optionText.Split(" x ");
        int width = int.Parse(textOut[0]);
        int height = int.Parse(textOut[1]);

        Screen.SetResolution(width, height, screenMode);
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

    public void MasterChange(Slider slider)
    {
        SoundManager.SettingMaster(slider.value);
    }
}
