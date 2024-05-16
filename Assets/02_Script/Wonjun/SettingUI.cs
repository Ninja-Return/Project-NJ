using DG.Tweening;
using Michsky.UI.Dark;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using Unity.Services.Vivox;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SettingUI : MonoBehaviour
{
    [SerializeField] private Slider sensitivitySlider;
    public MainPanelManager mainPanelManager;

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
        sensitivitySlider.onValueChanged.AddListener(delegate { UpdateSensitivity(); });
    }

    void Update()
    {
        if (!PlayerManager.Instance.active) return;

        if (Input.GetKeyDown(KeyCode.Escape) && !Setting)
        {
            SettingOn();
        }
    }

    public void SettingOn()
    {
        Support.SettingCursorVisable(isShow);

        if (PlayerManager.Instance == null)
            playerController.Active(!isShow);
        else
            PlayerManager.Instance.localController.Active(!isShow);

        mainPanelManager.OpenPanel("Settings");

        Setting = true;
    }

    public void SettingOff()
    {
        Support.SettingCursorVisable(!isShow);

        if (PlayerManager.Instance == null)
            playerController.Active(isShow);
        else
            PlayerManager.Instance.localController.Active(isShow);

        mainPanelManager.OpenFirstTab();

        Setting = false;
    }

    public void ModeWindowedScreen()
    {
        screenMode = UnityEngine.FullScreenMode.Windowed;

        int width = Screen.width;
        int height = Screen.height;
        Screen.SetResolution(width, height, screenMode);
    }

    public void ModeFullScreen()
    {
        screenMode = UnityEngine.FullScreenMode.FullScreenWindow;

        int width = Screen.width;
        int height = Screen.height;
        Screen.SetResolution(width, height, screenMode);
    }

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

        if (NetworkManager.Singleton.IsHost)
        {

            HostSingle.Instance.GameManager.ShutdownAsync();

        }
        else
        {

            ClientSingle.Instance.GameManager.Disconnect();

        }

        SceneManager.LoadScene(SceneList.LobbySelectScene);
    }

    public void UpdateSensitivity()
    {

        PlayerPrefs.SetFloat("LookSenc", sensitivitySlider.value);

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

    public void SetVoice(float value)
    {

        VivoxService.Instance.SetOutputDeviceVolume((int)value - 50);

    }

}
