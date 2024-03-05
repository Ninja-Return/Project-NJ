using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerKillState : PlayerStateRoot
{

    private TMP_Text interactionText;
    private Transform cameraTrm;
    private PlayerController player;
    private PlayerHand hand;

    public PlayerKillState(PlayerController controller) : base(controller)
    {

        interactionText = transform.Find("InteractionCanvas").Find("KillText").GetComponent<TMP_Text>();
        cameraTrm = transform.Find("PlayerCamera");
        interactionText.text = string.Empty;
        hand = GetComponent<PlayerHand>();

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

            if(hand.CheckHandItem("검 아이템 키"))
            {

                GameManager.Instance.PlayerDieServerRPC(player.OwnerClientId);

            }
            else
            {

                Debug.Log("위치 제공");

            }


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


            if (player != null)
            {

                if(hand.CheckHandItem("검 아이템 키"))
                {

                    interactionText.text = "E키를 눌러 플레이어를 죽이세요";

                }
                else
                {

                    interactionText.text = "E키를 몬스터에게 위치를 제공하세요";

                }


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
