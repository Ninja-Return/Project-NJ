using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerKillState : PlayerStateRoot
{

    private TMP_Text interactionText;
    private Transform cameraTrm;
    private PlayerController player;

    public PlayerKillState(PlayerController controller) : base(controller)
    {

        interactionText = transform.Find("InteractionCanvas").Find("InteractionText").GetComponent<TMP_Text>();
        cameraTrm = transform.Find("PlayerCamera");
        interactionText.text = string.Empty;

    }

    protected override void EnterState()
    {

        input.OnInteractionKeyPress += HandleInteractionKeyPress;

    }



    protected override void ExitState()
    {

        input.OnInteractionKeyPress -= HandleInteractionKeyPress;

    }


    private void HandleInteractionKeyPress()
    {

        if (player != null)
        {

            GameManager.Instance.PlayerDieServerRPC(player.OwnerClientId);

        }

    }

    protected override void UpdateState()
    {

        CheckInteraction();

    }

    private void CheckInteraction()
    {

        var hit = Physics.Raycast(cameraTrm.position, cameraTrm.forward, 
            out var info, data.InteractionRange.Value, LayerMask.GetMask("Player"));

        if (hit)
        {

            player = info.transform.GetComponent<PlayerController>();

            Debug.Log(123);

            if (player != null)
            {

                interactionText.text = "E키를 눌러 플레이어를 죽이세요";

            }
            else
            {

                interactionText.text = string.Empty;

            }

        }
        else
        {

            interactionText.text = string.Empty;
            player = null;

        }

    }

}
