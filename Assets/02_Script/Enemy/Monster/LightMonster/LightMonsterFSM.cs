using FSM_System.Netcode;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public enum LigthMonsterStateType
{

    Patrol,
    Chase,
    Stun,
    Attack,

}

#region States

public abstract class LightMonsterBaseState : FSM_State_Netcode<LigthMonsterStateType>
{

    protected new LightMonsterFSM controller;

    protected LightMonsterBaseState(FSM_Controller_Netcode<LigthMonsterStateType> controller) : base(controller)
    {

        this.controller = controller as LightMonsterFSM;

    }

    protected Vector3? GetRandomNavMeshPoint(float distance, int sampleCount = 30)
    {

        List<Vector3> list = new();

        for(int i = 0; i < sampleCount; i++)
        {

            var rpos = Random.insideUnitSphere * distance;
            rpos.y = transform.position.y;

            if(NavMesh.SamplePosition(rpos, out var hit, 2, NavMesh.AllAreas))
            {

                list.Add(hit.position);

            }

        }

        return list.Count == 0 ? null : list[Random.Range(0, list.Count)];

    }

    protected List<Vector3> GetRandomNavMeshPoints(float distance, int sampleCount = 30)
    {

        List<Vector3> list = new();

        for (int i = 0; i < sampleCount; i++)
        {

            var rpos = Random.insideUnitSphere * distance;
            rpos.y = transform.position.y;

            if (NavMesh.SamplePosition(rpos, out var hit, 2, NavMesh.AllAreas))
            {

                list.Add(hit.position);

            }

        }

        return list;

    }

}

public class LightMonsterPatrolState : LightMonsterBaseState
{
    public LightMonsterPatrolState(FSM_Controller_Netcode<LigthMonsterStateType> controller) : base(controller)
    {
    }

    private Coroutine coroutine;

    protected override void EnterState()
    {

        controller.SetSpeed(5);
        coroutine = StartCoroutine(PatrolCo());
        controller.ControlAnimator.SetIsWalk(true);

    }

    protected override void ExitState()
    {

        if(coroutine != null)
        {

            StopCoroutine(coroutine);

        }

        coroutine = null;

        controller.ControlAnimator.SetIsWalk(false);

    }

    private IEnumerator PatrolCo()
    {

        while (true)
        {


            var pos = GetRandomNavMeshPoint(50, 70);

            if(pos != null)
            {

                controller.MoveController.Move(pos.Value);

            }

            yield return new WaitUntil(() => !controller.MoveController.HasPath);

        }

    }

}

public class LightMonsterChaseState : LightMonsterBaseState
{
    public LightMonsterChaseState(FSM_Controller_Netcode<LigthMonsterStateType> controller) : base(controller)
    {
    }

    protected override void EnterState()
    {

        controller.SetSpeed(8);
        controller.ControlAnimator.SetIsRun(true);

    }

    protected override void ExitState()
    {

        controller.ControlAnimator.SetIsRun(false);

    }

    protected override void UpdateState()
    {

        controller.MoveController.Move(controller.Target.position);

    }

}

public class LightMonsterStunState : LightMonsterBaseState
{
    public LightMonsterStunState(FSM_Controller_Netcode<LigthMonsterStateType> controller) : base(controller)
    {
    }

    protected override void EnterState()
    {

        controller.ControlAnimator.SetStun();
        controller.ControlAnimator.OnAnimeEnd += HandleAnimeEnd;
        controller.MoveController.Stop();

    }

    protected override void ExitState()
    {

        controller.ControlAnimator.OnAnimeEnd -= HandleAnimeEnd;
        controller.MoveController.Continue();

        if(controller.Target == null)
        {

            controller.SetTarget(controller.MoveController.GetClosest(10, LayerMask.GetMask("Player")).transform);

        }

    }

    private void HandleAnimeEnd()
    {

        controller.ChangeState(LigthMonsterStateType.Chase);

    }

}

public class LightMonsterJumpAttackState : LightMonsterBaseState
{
    public LightMonsterJumpAttackState(FSM_Controller_Netcode<LigthMonsterStateType> controller) : base(controller)
    {
    }

}

public class LigthMonsterAttackState : LightMonsterBaseState
{
    public LigthMonsterAttackState(FSM_Controller_Netcode<LigthMonsterStateType> controller) : base(controller)
    {
    }

    protected override void EnterState()
    {

        controller.ControlAnimator.SetNormalAttack();
        controller.ControlAnimator.OnAnimeEnd += HandleEnd;
        controller.MoveController.Stop();

    }

    protected override void ExitState()
    {

        controller.ControlAnimator.OnAnimeEnd -= HandleEnd;
        controller.MoveController.Continue();

    }

    private void HandleEnd()
    {

        if(controller.Target != null)
        {

            if(Vector3.Distance(transform.position, controller.Target.transform.position) <= 3)
            {

                var id = controller.Target.GetComponent<PlayerController>().OwnerClientId;

                PlayerManager.Instance.PlayerDie(EnumList.DeadType.Dron, id);

            }

        }

        controller.ChangeState(LigthMonsterStateType.Patrol);

    }
}

#endregion

#region Transitions

public abstract class LightMonsterTransitionBase : FSM_Transition_Netcode<LigthMonsterStateType>
{

    protected new LightMonsterFSM controller;

    protected LightMonsterTransitionBase(FSM_Controller_Netcode<LigthMonsterStateType> controller, LigthMonsterStateType nextState) : base(controller, nextState)
    {

        this.controller = controller as LightMonsterFSM;

    }

}

public class LightMonsterTargetCastingTransition : LightMonsterTransitionBase
{

    private bool checkNull = false;

    public LightMonsterTargetCastingTransition(FSM_Controller_Netcode<LigthMonsterStateType> controller, LigthMonsterStateType nextState, bool checkNull) : base(controller, nextState)
    {
        this.checkNull = checkNull;
    }

    protected override bool CheckTransition()
    {

        controller.CheckTargetting();

        return checkNull ? controller.Target == null : controller.Target != null;

    }

}

public class LightMonsterMoveEndTransition : LightMonsterTransitionBase
{
    public LightMonsterMoveEndTransition(FSM_Controller_Netcode<LigthMonsterStateType> controller, LigthMonsterStateType nextState) : base(controller, nextState)
    {
    }

    protected override bool CheckTransition()
    {

        return !controller.MoveController.IsMoving;

    }

}

public class LightMonsterLightTransition : LightMonsterTransitionBase
{

    private int lightCastedCount;

    public LightMonsterLightTransition(FSM_Controller_Netcode<LigthMonsterStateType> controller, LigthMonsterStateType nextState) : base(controller, nextState)
    {


    }

    public override void EnterTransition()
    {

        lightCastedCount = 0;
        controller.OnCastedEvent += HandleLightCasted;

    }

    private void HandleLightCasted()
    {

        lightCastedCount++;

    }

    public override void ExitTransition()
    {

        controller.OnCastedEvent -= HandleLightCasted;

    }

    protected override bool CheckTransition()
    {

        return lightCastedCount > 0;

    }

}

public class LightMonsterRangeTransition : LightMonsterTransitionBase
{

    private float range;

    public LightMonsterRangeTransition(FSM_Controller_Netcode<LigthMonsterStateType> controller, LigthMonsterStateType nextState, float range) : base(controller, nextState)
    {
        this.range = range;
    }

    protected override bool CheckTransition()
    {

        if (controller.Target == null) return false;

        return Vector3.Distance(transform.position, controller.Target.position) <= range;

    }

}

#endregion

[RequireComponent(typeof(MonsterController))]
[RequireComponent(typeof(LightMonsterAnimater))]
public class LightMonsterFSM : FSM_Controller_Netcode<LigthMonsterStateType>, ILightCastable
{

    [SerializeField] private LayerMask targetMask;
    [SerializeField] private LayerMask obstacleMask;
    [SerializeField] private float angle;

    public LightMonsterAnimater ControlAnimator { get; private set; }
    public MonsterController MoveController { get; private set; }
    public Transform Target { get; private set; }

    public event Action OnCastedEvent;

    protected override void Awake()
    {

        InitComponent();
        InitState();

        base.Awake();

    }

    private void Start()
    {

        ChangeState(currentState);

    }

    private void InitState()
    {

        var patrol = new LightMonsterPatrolState(this);
        var chase = new LightMonsterChaseState(this);
        var stun = new LightMonsterStunState(this);
        var attack = new LigthMonsterAttackState(this);

        var goRun = new LightMonsterLightTransition(this, LigthMonsterStateType.Stun);
        var goChase = new LightMonsterTargetCastingTransition(this, LigthMonsterStateType.Chase, false);
        var goPatrol = new LightMonsterTargetCastingTransition(this, LigthMonsterStateType.Patrol, true);
        var goAttack = new LightMonsterRangeTransition(this, LigthMonsterStateType.Attack, 3);

        patrol.AddTransition(goRun);
        patrol.AddTransition(goChase);

        chase.AddTransition(goRun);
        chase.AddTransition(goPatrol);
        chase.AddTransition(goAttack);

        AddState(patrol, LigthMonsterStateType.Patrol);
        AddState(chase, LigthMonsterStateType.Chase);
        AddState(stun, LigthMonsterStateType.Stun);
        AddState(attack, LigthMonsterStateType.Attack);

    }

    protected override void Update()
    {

        if (!IsServer) return;

        base.Update();

    }

    private void InitComponent()
    {

        MoveController = GetComponent<MonsterController>(); 
        ControlAnimator = GetComponent<LightMonsterAnimater>();

    }

    public void Casting(Vector3 trm)
    {

        var dir = trm - transform.position;

        if(Vector3.Dot(transform.forward, dir) > 0)
        {

            OnCastedEvent?.Invoke();

        }

    }

    public void CheckTargetting()
    {

        var target = MoveController.ViewingAndGetClosest(30, angle, targetMask, obstacleMask);

        if(target != null)
        {

            Target = target.transform;

        }
        else
        {

            Target = null;

        }

    }

    public void SetTarget(Transform trm)
    {

        Target = trm;

    }

    public void SetSpeed(float speed)
    {

        MoveController.MonsterAgnet.speed = speed;

    }

    public void RemovingTarget()
    {

        Target = null;

    }

}
