using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SellObjectSys : InteractionObject
{

    [SerializeField] private Transform moveObject;
    [SerializeField] private SellSystem sellSystem;

    protected override void DoInteraction()
    {

        Sequence seq = DOTween.Sequence();
        seq.Append(moveObject.DOLocalMoveZ(-0.6f, 0.3f).SetEase(Ease.OutQuad));
        seq.AppendInterval(1);
        seq.AppendCallback(Selling);
        seq.AppendInterval(1);
        seq.Append(moveObject.DOLocalMoveZ(0.47f, 0.3f).SetEase(Ease.OutQuad));

    }

    private void Selling()
    {

        sellSystem.Sell(NetworkManager.LocalClientId);

    }

}
