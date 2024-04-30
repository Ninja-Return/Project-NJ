using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class SellObjectSys : InteractionObject
{

    [SerializeField] private Transform moveObject;
    [SerializeField] private SellSystem sellSystem;

    private bool isLock;

    protected override void DoInteraction()
    {

        if (isLock) return;
        isLock = true;

        MoveServerRPC(NetworkManager.LocalClientId);

    }

    [ServerRpc(RequireOwnership = false)]
    private void MoveServerRPC(ulong clickClientId)
    {

        Sequence seq = DOTween.Sequence();
        seq.Append(moveObject.DOLocalMoveZ(-0.6f, 0.3f).SetEase(Ease.OutQuad));
        seq.AppendInterval(1);
        seq.AppendCallback(() => Selling(clickClientId));
        seq.AppendInterval(1);
        seq.Append(moveObject.DOLocalMoveZ(0.47f, 0.3f).SetEase(Ease.OutQuad));

    }

    private void Selling(ulong clickClientId)
    {

        sellSystem.Sell(clickClientId);
        isLock = false;

    }

}
