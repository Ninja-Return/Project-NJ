using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class PlayerAnimationController : NetworkBehaviour
{

    private readonly int HASH_IS_GROUND = Animator.StringToHash("IsGround");
    private readonly int HASH_SITDOWN = Animator.StringToHash("Sit");
    private readonly int HASH_X = Animator.StringToHash("X");
    private readonly int HASH_Y = Animator.StringToHash("Y");
    private readonly int HASH_EMOTION = Animator.StringToHash("Emotion");

    [SerializeField] private bool debug;
    [SerializeField] private List<GameObject> tweenAnimationClient = new();
    [SerializeField] private List<GameObject> tweenAnimationServer = new();

    private Dictionary<string, List<DOTweenAnimation>> clientTweenContainer = new();
    private Dictionary<string, List<DOTweenAnimation>> serverTweenContainer = new();
    private Vector3 originTargetPos;
    private Quaternion originTargetRot;

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

    private NetworkVariable<int> emotionStateValue =
        new NetworkVariable<int>(default,
            NetworkVariableReadPermission.Everyone,
            NetworkVariableWritePermission.Owner);

    private NetworkVariable<float> rigValue =
        new NetworkVariable<float>(default,
            NetworkVariableReadPermission.Everyone,
            NetworkVariableWritePermission.Owner);

    private PlayerController playerController;
    private Animator controlAnimator;
    private Transform handTarget;
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

        if (IsOwner)
        {
            //
            controlAnimator = clientObj.GetComponent<Animator>();
            controlRig = clientObj.Find("Rig 1").GetComponent<Rig>();
            handTarget = controlRig.transform.GetChild(0);
            playerController = GetComponent<PlayerController>();
            groundSencer = GetComponentInChildren<GroundSencer>();

            foreach(var item in tweenAnimationClient)
            {

                clientTweenContainer.Add(item.name, item.GetComponents<DOTweenAnimation>().ToList());

            }

        }
        else
        {

            controlAnimator = serverObj.GetComponent<Animator>();
            controlRig = serverObj.Find("Rig 1").GetComponent<Rig>();
            handTarget = controlRig.transform.GetChild(0);

            xStateValue.OnValueChanged += HandleXValueChanged;
            yStateValue.OnValueChanged += HandleYValueChanged;
            isGroundStateValue.OnValueChanged += HandleIsGroundChanged;
            rigValue.OnValueChanged += HandleRigValueChanged;
            sitDownStateValue.OnValueChanged += HandleSitDownChanged;
            emotionStateValue.OnValueChanged += HandleSocialAction;

            foreach (var item in tweenAnimationServer)
            {

                serverTweenContainer.Add(item.name, item.GetComponents<DOTweenAnimation>().ToList());

            }


        }

        originTargetPos = handTarget.transform.localPosition;
        originTargetRot = handTarget.transform.localRotation;

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

        emotionStateValue.Value = playerController.onEmotionkeyNumber;

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

    private void HandleSocialAction(int previousValue, int newValue)
    {

        controlAnimator.SetFloat(HASH_EMOTION, newValue);

    }

    public void HandControl(bool isUp)
    {

        var value = isUp ? 1 : 0;
        controlRig.weight = value;
        rigValue.Value = value;

    }

    public void InitHandTarget()
    {

        handTarget.transform.localPosition = originTargetPos;
        handTarget.transform.localRotation = originTargetRot;

    }

    public void PlayTweenAnimation(string key)
    {

        if (IsOwner)
        {

            foreach (var tween in clientTweenContainer[key])
            {

                tween.CreateTween();

            }


        }

        TweenAnimeServerRPC(key);

    }

    [ServerRpc]
    private void TweenAnimeServerRPC(string key)
    {

        TweenAnimeClientRPC(key);
    }

    [ClientRpc]
    private void TweenAnimeClientRPC(string key)
    {

        if (IsOwner) return;

        foreach (var tween in serverTweenContainer[key])
        {

            tween.CreateTween();

        }

    }

    public Transform GetHandTarget()
    {

        return controlRig.transform.Find("HandTarget");

    }

}
