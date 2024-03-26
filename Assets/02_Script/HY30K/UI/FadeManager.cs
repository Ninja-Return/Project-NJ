using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeManager : MonoBehaviour
{
    [SerializeField] private CanvasGroup canvasGroup;

    public static FadeManager Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    public void FadeOn()
    {
        canvasGroup.DOFade(1, 2);
    }
}
