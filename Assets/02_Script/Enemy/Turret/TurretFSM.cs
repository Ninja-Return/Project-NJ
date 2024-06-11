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
    public Transform headTrs;
    public Transform fireTrs;
    public GameObject lightObj;
    public LineRenderer lineRenderer;
    public ParticleSystem fireParticle;

    [SerializeField] private Missile missileObj;
    [SerializeField] private float range;
    [SerializeField] private float angle;
    [SerializeField] private float raderTime;
    [SerializeField] private LayerMask playerMask, obstacle;

    [HideInInspector] public Transform playerTrs;

    public TurretState nowState;

    private void Start()
    {
        if (!IsServer) return;

        base.Awake();

        InitializeStates();
        ChangeState(TurretState.Idle);

        lineRenderer.enabled = false;
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
            var dest = Vector3.Distance(hit.point, pos);

            if(Physics.Raycast(pos, lookDir, dest, obstacle))
            {

                return null;

            }

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
        Missile missile = Instantiate(missileObj, fireTrs.position, Quaternion.Euler(90, 0, 0));
        missile.NetworkObject.Spawn(true);

        Vector3 dir = (playerTrs.position - fireTrs.position).normalized;
        missile.FireMove(dir);
    }
}
