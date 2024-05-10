using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using EnumList;
using TMPro;
using DG.Tweening;
using Unity.Netcode;

[System.Serializable]
public struct MessageData
{
    public DeadType deadType;
    public string deadStr;
    public string signStr;
}

public class DeathUISystem : MonoBehaviour
{
    public static DeathUISystem Instance;

    [SerializeField] private List<MessageData> messageDatas;

    [Header("Obj")]
    [SerializeField] private GameObject panelObj;
    [SerializeField] private Image fadeImage;
    [SerializeField] private TextMeshProUGUI deadText;
    [SerializeField] private TextMeshProUGUI signText;

    private void Awake()
    {
        Instance = this;
    }

    public void PopupDeathUI(DeadType deadType)
    {
        MessageData message = messageDatas.Find(x => x.deadType == deadType);
        deadText.text = message.deadStr;
        signText.text = message.signStr;

        if (deadType == DeadType.Escape)
        {
            fadeImage.color = Color.blue;
        }

        panelObj.SetActive(true);

        Sequence seq = DOTween.Sequence();
        seq.Append(fadeImage.DOColor(Color.black, 1.5f)).
            AppendInterval(1.5f).
            OnComplete(() => 
            {
                panelObj.SetActive(false);
            });
    }
}
