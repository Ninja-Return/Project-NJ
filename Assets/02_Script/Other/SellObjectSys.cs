using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class SellObjectSys : InteractionObject
{

    [SerializeField] private Transform moveObject;
    [SerializeField] private SellSystem sellSystem;

    private NetworkVariable<bool> locked;

    public override void OnNetworkSpawn()
    {

        locked = new();

    }

    protected override void DoInteraction()
    {

        if (locked.Value) return;

        NetworkSoundManager.Play3DSound("BoxOpen", transform.position, 0.1f, 10f);

        MoveServerRPC(NetworkManager.LocalClientId);

    }

    [ServerRpc(RequireOwnership = false)]
    private void MoveServerRPC(ulong buyClientId)
    {

        locked.Value = true;

        Sequence seq = DOTween.Sequence();
        seq.Append(moveObject.DOLocalMoveZ(-0.6f, 0.3f).SetEase(Ease.OutQuad));
        seq.AppendInterval(1);
        seq.AppendCallback(() => Selling(buyClientId));
        seq.AppendInterval(1)
            .OnComplete(() =>
            {
                NetworkSoundManager.Play3DSound("BoxClose", transform.position, 0.1f, 10f);
            });
        seq.Append(moveObject.DOLocalMoveZ(0.47f, 0.3f).SetEase(Ease.OutQuad));
        seq.AppendCallback(() =>
        {

            locked.Value = false;

        });

    }

    private void Selling(ulong buyClientId)
    {

        NetworkSoundManager.Play3DSound("Sell", transform.position, 0.01f, 10f);
        sellSystem.Sell(buyClientId);

    }

    public override void OnNetworkDespawn()
    {

        locked.Dispose();

    }

}
