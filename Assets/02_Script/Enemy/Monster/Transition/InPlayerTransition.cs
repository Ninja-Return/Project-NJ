using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FSM_System.Netcode;

public class InPlayerTransition : MonsterTransitionRoot
{
    private PlayerController player;
    private float radius;
    private bool inRader;
    private float currentTime;

    readonly float raderTime = 0.8f;

    public InPlayerTransition(MonsterFSM controller, MonsterState nextState, float radius) : base(controller, nextState) 
    {
        this.radius = radius;
    }

    protected override bool CheckTransition()
    {
        if (nav.pathPending) return false;

        List<Collider> chasePlayer = monsterFSM.ViewingPlayer(radius);
        if (chasePlayer != null)
        {
            if (!inRader)
            {
                inRader = true;
                NetworkSoundManager.Play3DSound("RaderPlayer", monsterFSM.transform.position, 0.1f, 45f, SoundType.SFX, AudioRolloffMode.Linear);
            }

            currentTime += Time.deltaTime;

            foreach (var item in chasePlayer)
            {
                monsterFSM.targetPlayer = item;
                player = item.GetComponent<PlayerController>(); //서버의 클라만 정보가 담기니

                Vector2 moveVec = PlayerManager.Instance.FindPlayerControllerToID(player.OwnerClientId).moveVector.Value;

                if (moveVec != Vector2.zero && currentTime > raderTime)
                {
                    currentTime = 0f;
                    return true;
                }
            }
        }
        else
        {
            inRader = false;
            currentTime = 0f;
        }

        return false;
    }


}
