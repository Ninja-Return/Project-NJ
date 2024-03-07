using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TvOpenText : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            OnTv();
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            OffTv();
        }
    }

    private void OnTv()
    {
        transform.localScale = new Vector3(0, 0, 1);

        Sequence tvSequence = DOTween.Sequence();
        tvSequence.Append(transform.DOScaleX(0.1f, 0.2f).SetEase(Ease.InCirc));
        tvSequence.Join(transform.DOScaleY(1f, 0.2f).SetEase(Ease.OutBack));
        tvSequence.Append(transform.DOScaleX(1f, 0.5f).SetEase(Ease.OutElastic));
    }

    private void OffTv()
    {
        transform.DOScale(new Vector3(0, 0, 1), 0.4f).SetEase(Ease.OutExpo);
    }
}
