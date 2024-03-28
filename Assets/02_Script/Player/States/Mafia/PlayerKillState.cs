using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;

public class PlayerKillState : PlayerStateRoot
{

    private TMP_Text interactionText;
    private Transform cameraTrm;
    private PlayerController player;
    private PlayerHand hand;
    private MonsterFSM monsterFSM;

    public PlayerKillState(PlayerController controller) : base(controller)
    {

        interactionText = transform.Find("InteractionCanvas").Find("KillText").GetComponent<TMP_Text>();
        cameraTrm = transform.Find("PlayerCamera");
        interactionText.text = string.Empty;
        hand = GetComponent<PlayerHand>();
        monsterFSM = Object.FindObjectOfType<MonsterFSM>();

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

            if(hand.CheckHandItem("³ì½¼ Ä®"))
            {

                PlayerManager.Instance.PlayerDie(EnumList.DeadType.Mafia, player.OwnerClientId);
                Inventory.Instance.Deleteltem();

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

                if(hand.CheckHandItem("³ì½¼ Ä®"))
                {

                    interactionText.text = "EÅ°¸¦ ´­·¯ ÇÃ·¹ÀÌ¾î¸¦ Á×ÀÌ¼¼¿ä";

                }
                else
                {

                    interactionText.text = "";

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
