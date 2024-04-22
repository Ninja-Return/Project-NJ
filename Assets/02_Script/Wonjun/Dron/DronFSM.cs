using Cinemachine;
using FSM_System.Netcode;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using DG.Tweening;
using Unity.Services.Lobbies.Models;

public enum DronState
{
    Idle,
    Patrol,
    Chase,
    Kill,
    Dead
}


public class DronFSM : FSM_Controller_Netcode<DronState>, IEnemyInterface
{
    public UnityEngine.AI.NavMeshAgent nav;
    public Transform headTrs;
    public CinemachineVirtualCamera jsVcamTrs;
    public float angle;
    public Vector3 lookVec;
    public LayerMask playerMask;

    public DronState nowState;

    [HideInInspector] public Vector3 pingPos;
    [HideInInspector] public Collider targetPlayer;
    [HideInInspector] public bool IsDead { get; private set; }
    [HideInInspector] public bool IsKill;

    [Header("Values")]
    [SerializeField] private float moveRadius;
    [SerializeField] private float chaseRadius;
    [SerializeField] private float killRadius;
    [SerializeField] private float workSpeed;
    [SerializeField] private float runSpeed;
    [SerializeField] private float lazerTime;
    [SerializeField] private float stopTime;
    [SerializeField] float shakeAmount = 3.0f;
    [SerializeField] float shakeTime = 1.0f;
    [SerializeField] private LayerMask obstacleMask;

    private void Start()
    {
        if (!IsHost)
        {
            nav.enabled = false;
        }

        if (!IsServer) return;

        base.Awake();

        InitializeStates();
        ChangeState(DronState.Idle);
    }

    private void InitializeStates()
    {
        DronIdleState dronIdleState = new DronIdleState(this);
        DronPatrolState dronPatrolState = new DronPatrolState(this, moveRadius, workSpeed);
        DronChaseState dronChaseState = new DronChaseState(this, chaseRadius, runSpeed, lazerTime, stopTime);
        DronKillState dronKillState = new DronKillState(this);
        DronDeathState dronDeathState = new DronDeathState(this);

        DronMoveTransition dronMoveTransition = new DronMoveTransition(this, DronState.Idle);
        DronInPlayerTransition dronChasePlayerTransition = new DronInPlayerTransition(this, DronState.Chase, chaseRadius);
        DronCatchPlayerTransition dronCatchPlayerTransition = new DronCatchPlayerTransition(this, DronState.Kill, killRadius);
        DronDieTransition dronDieTransition = new DronDieTransition(this, DronState.Dead);

        dronPatrolState.AddTransition(dronMoveTransition);

        dronIdleState.AddTransition(dronChasePlayerTransition);
        dronPatrolState.AddTransition(dronChasePlayerTransition);

        dronChaseState.AddTransition(dronCatchPlayerTransition);

        dronIdleState.AddTransition(dronDieTransition);
        dronPatrolState.AddTransition(dronDieTransition);
        dronChaseState.AddTransition(dronDieTransition);
        dronKillState.AddTransition(dronDieTransition);

        AddState(dronIdleState, DronState.Idle);
        AddState(dronPatrolState, DronState.Patrol);
        AddState(dronChaseState, DronState.Chase);
        AddState(dronKillState, DronState.Kill);
        AddState(dronDeathState, DronState.Dead);
    }

    private void FixedUpdate()
    {
        if (!IsServer) return;
        if (targetPlayer == null) return;

        Vector3 targetPlayerPos = targetPlayer.transform.position;
        Vector3 playerVec = (targetPlayerPos - transform.position).normalized;
        lookVec = playerVec;
    }




    private bool RayObstacle(Vector3 pos, Vector3 lookVec, float destance)
    {
        return Physics.Raycast(pos, lookVec, destance, obstacleMask);
    }

    public Collider RayPlayer(float radius)
    {
        Vector3 pos = headTrs.position;

        RaycastHit[] allPlayers = Physics.RaycastAll(pos, lookVec, radius, playerMask);
        if (allPlayers.Length == 0) return null;

        float minDistance = float.MaxValue;
        Collider targetPlayer = null;
        foreach (RaycastHit player in allPlayers)
        {
            if (Vector3.Distance(player.transform.position, pos) < minDistance)
            {
                targetPlayer = player.collider;
            }
        }

        Vector3 targetPos = targetPlayer.transform.position;
        Debug.DrawLine(pos, targetPos, Color.black);

        return targetPlayer;
    }

    public Collider CirclePlayer(float radius)
    {
        Vector3 pos = headTrs.position;

        Collider[] allPlayers = Physics.OverlapSphere(pos, radius, playerMask);
        if (allPlayers.Length == 0) return null;

        float minDistance = float.MaxValue;
        Collider targetPlayer = null;
        foreach (Collider player in allPlayers)
        {
            if (Vector3.Distance(player.transform.position, pos) < minDistance)
            {
                targetPlayer = player;
            }
        }

        //if (IsObstacle && RayObstacle(pos, lookVec, minDistance))
        //    return null;

        Vector3 targetPos = targetPlayer.transform.position;
        Debug.DrawLine(pos, targetPos, Color.red);

        return targetPlayer;
    }

    public Collider ViewingPlayer(float radius)
    {
        Vector3 pos = headTrs.position;
        Vector3 lookDir = headTrs.forward;

        // ÇÃ·¹ÀÌ¾î °¨Áö¸¦ À§ÇÑ ·¹ÀÌÄ³½ºÆ® ¹æÇâ
        RaycastHit[] hits = Physics.SphereCastAll(pos, radius, lookDir, 0f, playerMask);

        // °¨ÁöµÈ ÇÃ·¹ÀÌ¾î°¡ ¾øÀ¸¸é null ¹ÝÈ¯
        if (hits.Length == 0) return null;

        // °¨ÁöµÈ ÇÃ·¹ÀÌ¾î Áß °¡Àå °¡±î¿î ÇÃ·¹ÀÌ¾î Ã£±â
        float minDistance = float.MaxValue;
        Collider nearestPlayer = null;
        foreach (RaycastHit hit in hits)
        {
            Collider playerCollider = hit.collider;
            float distance = Vector3.Distance(playerCollider.transform.position, pos);
            if (distance < minDistance)
            {
                minDistance = distance;
                nearestPlayer = playerCollider;
            }
        }

        // ¿¡µðÅÍ¿¡¼­ ·¹ÀÌ¸¦ ±×·ÁÁÖ´Â ÄÚµå
        Debug.DrawRay(pos, lookDir * radius, Color.blue);

        // °¡Àå °¡±î¿î ÇÃ·¹ÀÌ¾î ¹ÝÈ¯
        return nearestPlayer;
    }



    /*private Vector3 AngleToDirX(float angle, float xRotation)
    {
        float radian = angle * Mathf.Deg2Rad;
        float y = -Mathf.Sin(xRotation * Mathf.Deg2Rad); // Y ÃàÀ¸·Î ³»·Á°¥ °Å¸®¸¦ °è»ê
        return new Vector3(Mathf.Sin(radian), y, Mathf.Cos(radian));
    }

    private Vector3 AngleToDirY(float angle1, bool isUp, float xRotation)
    {
        float radian1 = angle1 * Mathf.Deg2Rad;
        float radian2 = (angle * 0.5f) * Mathf.Deg2Rad;
        float y = -Mathf.Sin(xRotation * Mathf.Deg2Rad); // Y ÃàÀ¸·Î ³»·Á°¥ °Å¸®¸¦ °è»ê
        Vector3 angleVec = isUp == true ? new Vector3(0f, Mathf.Sin(radian2), 0f) : new Vector3(0f, -Mathf.Sin(radian2), 0f);
        return new Vector3(Mathf.Sin(radian1), y, Mathf.Cos(radian1)) + angleVec;
    }*/

    
    

    public void JumpScare() //ï¿½ï¿½Ç»ï¿?ï¿½Ö´Ï¸ï¿½ï¿½Ì¼ï¿½ï¿½ï¿½ ï¿½Ç¾ï¿½ ï¿½Ö¾î¼­ ï¿½Ã·ï¿½ï¿½Ì¾î°¡ ï¿½ï¿½ ï¿½ï¿½ï¿½ï¿½ï¿½Ì°ï¿½ ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½ ï¿½Ù¶óº¸°ï¿½ ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½ ï¿½Ï¸ï¿½ ï¿½ï¿½ ï¿½ï¿½?
    {
        var player = targetPlayer.GetComponent<PlayerController>();

        JumpScareClientRPC(player.OwnerClientId);

    }

    [ClientRpc]
    private void JumpScareClientRPC(ulong clientId)
    {
        if (clientId != NetworkManager.LocalClientId) return;

        //PlayerController player = PlayerManager.Instance.FindPlayerControllerToID(playerId);
        //player == null ï¿½Ì°ï¿½ ï¿½Â´Âµï¿½
        jsVcamTrs.Priority = 500;
        transform.DOShakePosition(0.5f);
        StartCoroutine(Shake(shakeAmount, shakeTime));
        //player.Input.Disable();
        //player.enabled = false;
    }

    IEnumerator Shake(float ShakeAmount, float ShakeTime)
    {
        Vector3 originalPosition = jsVcamTrs.transform.position;

        headTrs.localRotation = Quaternion.Euler(30f, 0f, 0f);

        float timer = 0;
        while (timer <= ShakeTime)
        {
            jsVcamTrs.transform.position = originalPosition + (Vector3)UnityEngine.Random.insideUnitCircle * ShakeAmount;
            timer += Time.deltaTime;
            yield return null;
        }

        jsVcamTrs.transform.position = originalPosition;

        KillPlayerServerRPC();
        headTrs.localRotation = Quaternion.Euler(0f, 0f, 0f);
    }

    [ServerRpc(RequireOwnership = false)]
    public void KillPlayerServerRPC()
    {
        if (!IsServer) return;

        var player = targetPlayer.GetComponent<PlayerController>();

        PlayerManager.Instance.PlayerDie(EnumList.DeadType.Monster, player.OwnerClientId);
        IsKill = true;
    }


    public void Death()
    {
        if (!IsServer) return;

        IsDead = true;
    }


    protected override void Update()
    {

        if (!IsServer) return;

        nowState = currentState;

        base.Update();


    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.gray;
        Gizmos.DrawWireSphere(transform.position, moveRadius);
        Gizmos.DrawWireSphere(transform.position, killRadius);
    }
#endif
}
