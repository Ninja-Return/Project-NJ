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

        Collider targetPlayer = monsterFSM.ViewingPlayer(radius);
        if (targetPlayer != null)
        {
            if (!inRader)
            {
                inRader = true;
                NetworkSoundManager.Play3DSound("RaderPlayer", monsterFSM.transform.position, 0.1f, 45f, SoundType.SFX, AudioRolloffMode.Linear);
            }

            currentTime += Time.deltaTime;

            monsterFSM.targetPlayer = targetPlayer;
            player = targetPlayer.GetComponent<PlayerController>(); //서버의 클라만 정보가 담기니

            Vector2 moveVec = PlayerManager.Instance.players.Find(x => x.OwnerClientId == player.OwnerClientId).Input.MoveVecter;
            if (moveVec != Vector2.zero && currentTime > raderTime)
            {
                currentTime = 0f;
                return true;
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
