using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PipeHandle : InteractionObject
{

    [SerializeField] private GameObject pipeHandle;
    [SerializeField] private GameObject smokeObject;
    private bool isPipeBreak = false;

    public override void OnNetworkSpawn()
    {

        New_GameManager.Instance.OnGameStarted += HandleStarted;

    }

    private void HandleStarted()
    {

        if (IsServer)
        {

            StartCoroutine(PipeBreakCo());

        }

    }

    private IEnumerator PipeBreakCo()
    {

        while(true)
        {

            yield return new WaitForSeconds(Random.Range(60, 120));

            if (isPipeBreak)
            {

                yield return null;
                continue;

            }

            if(Random.value > 0.2f)
            {

                PipeBreakClientRPC();

            }


        }

    }

    private void Update()
    {

        if (isPipeBreak)
        {

            interactionAble = true;
            interactionText = "E키를 눌러 배관을 수리";

        }
        else
        {

            interactionAble = false;

        }

    }

    [ClientRpc]
    private void PipeBreakClientRPC()
    {

        isPipeBreak = true;
        pipeHandle.transform.DOLocalRotate(new Vector3(0, 0, 90f), 0.3f);
        smokeObject.SetActive(true);

    }

    [ServerRpc(RequireOwnership = false)]
    private void FixPipeServerRPC()
    {

        FixPipeClientRPC();

    }

    [ClientRpc]
    private void FixPipeClientRPC()
    {

        isPipeBreak = false;

        pipeHandle.transform.DOLocalRotate(Vector3.zero, 0.3f);
        smokeObject.SetActive(false);

    }

    protected override void DoInteraction()
    {

        FixPipeServerRPC();

    }

}
