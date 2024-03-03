using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using DG.Tweening;

public class NotificationUIController : MonoBehaviour
{

    [SerializeField] private TMP_Text notificationText;

    public void Notification(string message)
    {

        DOTween.Kill(notificationText);

        notificationText.text = message;

        Sequence seq = DOTween.Sequence();

        seq.Append(notificationText.DOFade(1, 0.7f));
        seq.AppendInterval(0.3f);
        seq.Append(notificationText.DOFade(0, 0.7f));

    }

}
