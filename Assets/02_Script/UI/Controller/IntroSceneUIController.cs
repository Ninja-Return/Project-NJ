using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class IntroSceneUIController : MonoBehaviour
{
    public void GameobjectActiveTrue(GameObject obj)
    {
        obj.SetActive(true);
    }

    public void GameobjectActiveFalse(GameObject obj)
    {
        obj.SetActive(false);
    }

    public void LobbySceneChange()
    {
        //네트워크 매니저에서 넘겨주는거 있었는데
    }

    public void BgmSetting(Slider slider)
    {

    }

    public void SfxSetting(Slider slider)
    {

    }

    public void LangaugeSetting()
    {

    }

    public void ResolutionSetting(TMP_Dropdown dropdown)
    {
        int value = dropdown.value;
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

    public void Quit()
    {
        Application.Quit();
    }
}
