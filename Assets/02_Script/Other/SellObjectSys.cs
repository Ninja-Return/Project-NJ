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

        NetworkSoundManager.Play3DSound("BoxOpen", transform.position, 0.1f, 10f);

        Sequence seq = DOTween.Sequence();
        seq.Append(moveObject.DOLocalMoveZ(-0.6f, 0.3f).SetEase(Ease.OutQuad));
        seq.AppendInterval(1);
        seq.AppendCallback(Selling);
        seq.AppendInterval(1)
            .OnComplete(() => 
            {
                NetworkSoundManager.Play3DSound("BoxClose", transform.position, 0.1f, 10f);
            });
        seq.Append(moveObject.DOLocalMoveZ(0.47f, 0.3f).SetEase(Ease.OutQuad));

    }

    private void Selling()
    {

        NetworkSoundManager.Play3DSound("Sell", transform.position, 0.01f, 10f);
        sellSystem.Sell(NetworkManager.LocalClientId);

    }

}
