using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerSoundController : NetworkBehaviour
{

    [SerializeField] private NetworkAudioSource walkSource;
    [SerializeField] private GroundSencer groundSencer;

    private PlayerController controller;

    private void Awake()
    {

        controller = GetComponent<PlayerController>();

    }

    private void Update()
    {

        if (IsOwner)
        {

            SetWalkSound();

        }


    }

    private void SetWalkSound()
    {


        if(controller.Input.MoveVecter != Vector2.zero &&
            controller.CurrentState == EnumPlayerState.Move &&
            groundSencer.IsGround &&
            walkSource.isPlaying == false)
        {

            walkSource.Play();

        }
        else if(walkSource.isPlaying == true && 
            (!groundSencer.IsGround ||
            controller.CurrentState != EnumPlayerState.Move ||
            controller.Input.MoveVecter == Vector2.zero))
        {

            walkSource.Stop();

        }

    }

}
