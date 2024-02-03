using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerAnimationController : NetworkBehaviour
{

    private readonly int HASH_IS_GROUND = Animator.StringToHash("IsGround");
    private readonly int HASH_X = Animator.StringToHash("X");
    private readonly int HASH_Y = Animator.StringToHash("Y");

    private NetworkVariable<float> xStateValue = 
        new NetworkVariable<float>(default, 
            NetworkVariableReadPermission.Everyone, 
            NetworkVariableWritePermission.Owner);

    private NetworkVariable<float> yStateValue =
        new NetworkVariable<float>(default,
            NetworkVariableReadPermission.Everyone,
            NetworkVariableWritePermission.Owner);

    private NetworkVariable<bool> isGroundStateValue =
        new NetworkVariable<bool>(default,
            NetworkVariableReadPermission.Everyone,
            NetworkVariableWritePermission.Owner);

    private PlayerController playerController;
    private Animator controllAnimator;
    private GroundSencer groundSencer;
    private Vector2 oldInput;
    private bool oldIsGround;

    public override void OnNetworkSpawn()
    {

        var serverObj = transform.Find("VisualServer");
        var clientObj = transform.Find("VisualClient");

        serverObj.gameObject.SetActive(!IsOwner);
        clientObj.gameObject.SetActive(IsOwner);

        if (IsOwner)
        {

            controllAnimator = clientObj.GetComponent<Animator>();
            playerController = GetComponent<PlayerController>();
            groundSencer = GetComponentInChildren<GroundSencer>();

        }
        else
        {

            controllAnimator = serverObj.GetComponent<Animator>();

            xStateValue.OnValueChanged += HandleXValueChanged;
            yStateValue.OnValueChanged += HandleYValueChanged;
            isGroundStateValue.OnValueChanged += HandleIsGroundChanged;

        }

    }

    private void Update()
    {

        if (!IsOwner) return;

        if (oldInput != playerController.Input.MoveVecter)
        {

            xStateValue.Value = playerController.Input.MoveVecter.x;
            yStateValue.Value = playerController.Input.MoveVecter.y;

            controllAnimator.SetFloat(HASH_X, xStateValue.Value);
            controllAnimator.SetFloat(HASH_Y, yStateValue.Value);

            oldInput = playerController.Input.MoveVecter;

        }

        if(oldIsGround != groundSencer.IsGround)
        {

            controllAnimator.SetBool(HASH_IS_GROUND, groundSencer.IsGround);
            oldIsGround = groundSencer.IsGround;
            isGroundStateValue.Value = groundSencer.IsGround;

        }

    }

    private void HandleXValueChanged(float previousValue, float newValue)
    {

        controllAnimator.SetFloat(HASH_X, newValue);

    }

    private void HandleYValueChanged(float previousValue, float newValue)
    {

        controllAnimator.SetFloat(HASH_Y, newValue);

    }

    private void HandleIsGroundChanged(bool previousValue, bool newValue)
    {

        controllAnimator.SetBool(HASH_IS_GROUND, newValue);

    }

}
