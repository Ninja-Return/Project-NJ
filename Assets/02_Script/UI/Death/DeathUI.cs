using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using EnumList;
using TMPro;
using DG.Tweening;
using System.ComponentModel;

public class DeathUI : MonoBehaviour
{
    [SerializeField] private Image redImage;
    [SerializeField] private TextMeshProUGUI signText;

    const string monsterSign = "You were caught by a monster.";
    const string mafiaSign = "You were killed by the mafia.";
    const string voteSign = "You've been voted out and executed!";
    const string bugSign = "Well, this seems to be a bug;;";

    //�⺻���� UI����� ũ�� �Ѵ°� ���߿� controll�� �ҰŴϱ� �ּ�ó��

    public void PopupDeathUI(DeadType deadType)
    {
        //InputSystem�� �ƹ�Ŭ�����ٰ� PopupWatchingUI()�߰��ϱ�

        redImage.color = Color.red;
        redImage.DOColor(Color.black, 0.5f)
            .OnComplete(() =>
            {

                PopupWatchingUI();

            });

        signText.text = deadType switch
        {
            DeadType.Mafia => mafiaSign,
            DeadType.Monster => monsterSign,
            DeadType.Vote => voteSign,
            _ => bugSign
        };

        WatchingSystem.Instance.StartWatching();

    }



    public void PopupWatchingUI()
    {
        gameObject.SetActive(false);
        WatchingSystem.Instance.StartWatching();
    }

}
