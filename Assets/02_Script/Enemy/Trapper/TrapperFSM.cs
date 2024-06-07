using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Unity.Netcode;
using FSM_System.Netcode;
using TMPro;

public enum TrapperState
{
    Move,
    Dead
}

public class TrapperFSM : FSM_Controller_Netcode<TrapperState>, IEnemyInterface
{
    public MonsterAnimation trapperAnim;
    public NavMeshAgent nav;
    public NetworkObject playerDeadbodyPrefab;

    [SerializeField] private TMP_Text namePlate;
    [SerializeField] private Transform trapSpawnTrs;
    [SerializeField] private TrapDrop trapObj;
    [SerializeField] private float radius;
    [SerializeField] private float minTrapTime, maxTrapTime;

    public TrapperState nowState;

    [HideInInspector] public bool IsDead { get; private set; }

    private void Start()
    {
        if (!IsServer)
        {
            nav.enabled = false;
            return;
        }

        base.Awake();

        InitializeStates();
        NameSetting();
        ChangeState(TrapperState.Move);
    }

    private void InitializeStates()
    {
        TrapperMoveState trapperMoveState = new TrapperMoveState(this, radius, minTrapTime, maxTrapTime);
        TrapperDieState trapperDieState = new TrapperDieState(this);

        TrapperDieTransition trapperDieTransition = new TrapperDieTransition(this, TrapperState.Dead);

        trapperMoveState.AddTransition(trapperDieTransition);
        trapperDieState.AddTransition(trapperDieTransition);

        AddState(trapperMoveState, TrapperState.Move);
        AddState(trapperDieState, TrapperState.Dead);
    }

    private void NameSetting()
    {
        int idx = Random.Range(0, NetworkManager.ConnectedClientsIds.Count);
        ulong playerId = NetworkManager.ConnectedClientsIds[idx];
        string playerName = HostSingle.Instance.NetServer.GetUserDataByClientID(playerId).Value.nickName;

        NameSettingClientRpc(playerName);
    }

    protected override void Update()
    {
        namePlate.transform.LookAt(PlayerManager.Instance.localController.transform);

        if (!IsServer) return;

        nowState = currentState;

        base.Update();
    }

    public void SpawnTrap()
    {
        var obj = Instantiate(trapObj, trapSpawnTrs.position, Quaternion.identity);
        obj.NetworkObject.Spawn(true);

        float lookingAngle = trapSpawnTrs.eulerAngles.y;
        Vector3 lookDir = AngleToDirX(lookingAngle);
        obj.SetUp(lookDir);
    }

    private Vector3 AngleToDirX(float angle)
    {
        float radian = angle * Mathf.Deg2Rad;
        return new Vector3(Mathf.Sin(radian), 0, Mathf.Cos(radian));
    }

    public void Death()
    {
        DeathServerRpc();
    }

    [ServerRpc(RequireOwnership = false)]
    private void DeathServerRpc()
    {
        //IsDead = true;

        NetworkObject deathBody = Instantiate(playerDeadbodyPrefab, transform.position, transform.rotation);
        deathBody.Spawn(true);
        NetworkObject.Despawn();
    }

    [ClientRpc]
    private void NameSettingClientRpc(string name)
    {
        namePlate.text = name;
    }
}
