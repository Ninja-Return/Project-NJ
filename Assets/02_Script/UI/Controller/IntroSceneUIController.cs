using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using UnityEngine.SceneManagement;
using Unity.Netcode;

public class IntroSceneUIController : MonoBehaviour
{
    [SerializeField] private Image introPanel;
    [SerializeField] private TMP_Text nameText;

    private FullScreenMode screenMode = FullScreenMode.FullScreenWindow;

    private void Start()
    {
        Sequence introSequence = DOTween.Sequence();
        introSequence.Append(transform.DOLocalMove(Vector2.zero, 1f));
        introSequence.Append(introPanel.DOColor(Color.white, 0.1f));
        introSequence.Append(introPanel.DOFade(0, 0.5f));
        introSequence.Append(transform.DOLocalMove(Vector2.zero, 0.1f).OnComplete(() => { introPanel.gameObject.SetActive(false); }));

        if (PlayerPrefs.GetString("PlayerName") == "")
        {
            PlayerPrefs.SetString("PlayerName", $"Player{Random.Range(1, 1000)}");
        }

        nameText.text = PlayerPrefs.GetString("PlayerName");
        nameText.text = "�̿�";
    }

    public void GameobjectActiveTrue(GameObject obj)
    {
        obj.SetActive(true);
    }

    public void GameobjectActiveFalse(GameObject obj)
    {
        obj.SetActive(false);
    }

    public void PanelMoveOn(RectTransform rectTrs)
    {
        rectTrs.DOLocalMoveY(0, 0.5f);
    }

    public void PanelMoveOut(RectTransform rectTrs)
    {
        rectTrs.DOLocalMoveY(-1200, 0.5f);
    }

    public void SceneChage()
    {
        CHSceneManager.Instance.ChangeScene(SceneList.LobbySelectScene);
    }

    public void BgmSetting(Slider slider)
    {
        Debug.Log("11");
        SoundManager.SettingBgm(slider.value);
    }

    public void SfxSetting(Slider slider)
    {
        SoundManager.SettingSfx(slider.value);
    }

    public void LangaugeSetting()
    {

    }

    public void FullScreenSetting(GameObject iconObj)
    {
        bool nowFullScreen = screenMode == FullScreenMode.Windowed ? true : false;
        screenMode = nowFullScreen == true ? FullScreenMode.FullScreenWindow : FullScreenMode.Windowed;
        iconObj.SetActive(nowFullScreen);

        int width = Screen.width;
        int height = Screen.height;
        Screen.SetResolution(width, height, screenMode);
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

    public void ScrollText(Scrollbar scrollbar)
    {
        scrollbar.value = 1f;

        DOTween.KillAll();
        Sequence sequence = DOTween.Sequence();

        sequence.AppendInterval(2f);
        sequence.Append(DOTween.To(() => scrollbar.value, x => scrollbar.value = x, 0f, 10f)
            .SetEase(Ease.Linear));

        sequence.Play();
    }

    public void NameSetting(TMP_Text newNameText)
    {
        Debug.Log(newNameText.text);
        PlayerPrefs.SetString("PlayerName", newNameText.text);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
