using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Unity.Netcode;
using FSM_System.Netcode;

public enum TurretState
{
    Idle,
    Rader,
    Fire
};

public class TurretFSM : FSM_Controller_Netcode<TurretState>
{
    //미사일
    public Transform headTrs;
    //빛
    //발사 파티클

    [SerializeField] private float range;
    [SerializeField] private float angle;
    [SerializeField] private float raderTime;
    [SerializeField] private LayerMask playerMask;

    [HideInInspector] public Transform playerTrs;

    public TurretState nowState;

    private void Start()
    {
        if (!IsServer) return;

        base.Awake();

        InitializeStates();
        ChangeState(TurretState.Idle);

        transform.Rotate(0f, Random.Range(0f, 360f), 0f);
    }

    private void InitializeStates()
    {
        TurretIdleState turretIdleState = new TurretIdleState(this, angle);
        TurretRaderState turretRaderState = new TurretRaderState(this, raderTime);
        TurretFireState turretFireState = new TurretFireState(this);

        AddState(turretIdleState, TurretState.Idle);
        AddState(turretRaderState, TurretState.Rader);
        AddState(turretFireState, TurretState.Fire);
    }

    protected override void Update()
    {
        if (!IsServer) return;

        nowState = currentState;

        base.Update();
    }

    public Collider ViewingPlayer()
    {
        float lookingAngle = headTrs.eulerAngles.y;
        Vector3 pos = headTrs.position;
        Vector3 lookDir = AngleToDirX(lookingAngle);

        Ray ray = new Ray(pos, lookDir);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, range, playerMask))
        {
            Collider targetPlayer = hit.collider;
            return targetPlayer;
        }

        return null;
    }

    private Vector3 AngleToDirX(float angle)
    {
        float radian = angle * Mathf.Deg2Rad;
        return new Vector3(Mathf.Sin(radian), 0, Mathf.Cos(radian));
    }

    public void FireMissile() //미사일 발사함수
    {
    }
}
