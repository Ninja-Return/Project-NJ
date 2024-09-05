using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class Gameover : MonoBehaviour
{
    [SerializeField] private List<CinemachineVirtualCamera> VCams;
    [SerializeField] private Image blackImg;

    private bool isFadeOut;

    private void Start()
    {
        Support.GetRandomListObject(VCams).Priority = 100;

        DOTween.Sequence()
            .AppendInterval(2f)
            .Append(blackImg.DOFade(0f, 2f))
            .OnComplete(() => { isFadeOut = true; });
    }

    private void Update()
    {
        if (Input.GetButtonDown("Fire1") && isFadeOut)
        {
            DOTween.Sequence()
                .Append(blackImg.DOFade(1f, 2f))
                .OnComplete(() => { SceneManager.LoadScene(SceneList.LobbySelectScene); });
        }
    }
}
