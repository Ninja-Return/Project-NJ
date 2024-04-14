using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class PlayerAnimationController : NetworkBehaviour
{

    private readonly int HASH_IS_GROUND = Animator.StringToHash("IsGround");
    private readonly int HASH_SITDOWN = Animator.StringToHash("Sit");
    private readonly int HASH_X = Animator.StringToHash("X");
    private readonly int HASH_Y = Animator.StringToHash("Y");

    [SerializeField] private bool debug;

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

    private NetworkVariable<bool> sitDownStateValue =
        new NetworkVariable<bool>(default,
            NetworkVariableReadPermission.Everyone,
            NetworkVariableWritePermission.Owner);

    private NetworkVariable<float> rigValue =
        new NetworkVariable<float>(default,
            NetworkVariableReadPermission.Everyone,
            NetworkVariableWritePermission.Owner);

    private PlayerController playerController;
    private Animator controlAnimator;
    private Rig controlRig;
    private GroundSencer groundSencer;
    private Vector2 oldInput;
    private bool oldIsGround;

    private void Awake()
    {

        if (debug)
        {

            var serverObj = transform.Find("VisualServer");
            groundSencer = GetComponentInChildren<GroundSencer>();
            controlAnimator = serverObj.GetComponent<Animator>();

        }

    }

    public override void OnNetworkSpawn()
    {

        var serverObj = transform.Find("VisualServer");
        var clientObj = transform.Find("PlayerCamera").Find("VisualClient");

        serverObj.gameObject.SetActive(!IsOwner);
        clientObj.gameObject.SetActive(IsOwner);

        //컨트롤 리그 널
        if (IsOwner)
        {

            controlAnimator = clientObj.GetComponent<Animator>();
            controlRig = clientObj.Find("Rig 1").GetComponent<Rig>();
            playerController = GetComponent<PlayerController>();
            groundSencer = GetComponentInChildren<GroundSencer>();

        }
        else
        {

            controlAnimator = serverObj.GetComponent<Animator>();
            controlRig = serverObj.Find("Rig 1").GetComponent<Rig>();

            xStateValue.OnValueChanged += HandleXValueChanged;
            yStateValue.OnValueChanged += HandleYValueChanged;
            isGroundStateValue.OnValueChanged += HandleIsGroundChanged;
            rigValue.OnValueChanged += HandleRigValueChanged;
            sitDownStateValue.OnValueChanged += HandleSitDownChanged;

        }

    }

    



    private void HandleRigValueChanged(float previousValue, float newValue)
    {

        controlRig.weight = newValue;

    }


    private void Update()
    {

        if (playerController == null) return;
        if ((!IsOwner && !debug) || playerController.CurrentState == EnumPlayerState.Idle)
        {
            xStateValue.Value = 0;
            yStateValue.Value = 0;
            controlAnimator.SetFloat(HASH_X, 0);
            controlAnimator.SetFloat(HASH_Y, 0);

        }

        if (!debug && oldInput != playerController.Input.MoveVecter)
        {

            xStateValue.Value = playerController.Input.MoveVecter.x;
            yStateValue.Value = playerController.Input.MoveVecter.y;

            controlAnimator.SetFloat(HASH_X, xStateValue.Value);
            controlAnimator.SetFloat(HASH_Y, yStateValue.Value);

            oldInput = playerController.Input.MoveVecter;

        }

        if(oldIsGround != groundSencer.IsGround)
        {

            controlAnimator.SetBool(HASH_IS_GROUND, groundSencer.IsGround);
            oldIsGround = groundSencer.IsGround;
            isGroundStateValue.Value = groundSencer.IsGround;

        }

        sitDownStateValue.Value = playerController.isSittingDown;
        HandleSitDownChanged(true, playerController.isSittingDown);

    }

    private void HandleSitDownChanged(bool previousValue, bool newValue)
    {

        controlAnimator.SetBool(HASH_SITDOWN, newValue);

    }

    private void HandleXValueChanged(float previousValue, float newValue)
    {

        controlAnimator.SetFloat(HASH_X, newValue);

    }

    private void HandleYValueChanged(float previousValue, float newValue)
    {

        controlAnimator.SetFloat(HASH_Y, newValue);

    }

    private void HandleIsGroundChanged(bool previousValue, bool newValue)
    {

        controlAnimator.SetBool(HASH_IS_GROUND, newValue);

    }

    public void HandControl(bool isUp)
    {

        var value = isUp ? 1 : 0;
        controlRig.weight = value;
        rigValue.Value = value;

    }

    public Transform GetHandTarget()
    {

        return controlRig.transform.Find("HandTarget");

    }

}
