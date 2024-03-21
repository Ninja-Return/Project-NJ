using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class AlertManager : MonoBehaviour
{
    [SerializeField] RectTransform panel;

    public void MoveAlert()
    {
        DOTween.Sequence()
             .Append(panel.DOLocalMoveY(0, 1))
             .Append(panel.DOLocalMoveY(1200, 1));
         
    }
}
