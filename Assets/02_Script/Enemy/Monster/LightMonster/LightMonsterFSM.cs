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
    RunAway

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

        coroutine = StartCoroutine(PatrolCo());

    }

    protected override void ExitState()
    {

        if(coroutine != null)
        {

            StopCoroutine(coroutine);

        }

        coroutine = null;

    }

    private IEnumerator PatrolCo()
    {

        while (true)
        {

            yield return new WaitUntil(() => !controller.MoveController.HasPath);

            var pos = GetRandomNavMeshPoint(50, 70);

            if(pos != null)
            {

                controller.MoveController.Move(pos.Value);

            }


        }

    }

}

public class LightMonsterChaseState : LightMonsterBaseState
{
    public LightMonsterChaseState(FSM_Controller_Netcode<LigthMonsterStateType> controller) : base(controller)
    {
    }

    protected override void UpdateState()
    {

        controller.MoveController.Move(controller.Target.position);

    }

}

public class LightMonsterRunAwayState : LightMonsterBaseState
{
    public LightMonsterRunAwayState(FSM_Controller_Netcode<LigthMonsterStateType> controller) : base(controller)
    {
    }

    protected override void EnterState()
    {

        var points = GetRandomNavMeshPoints(50, 100);
        var targetFwd = controller.Target == null ? transform.forward : controller.Target.forward;
        var targetPos = controller.Target == null ? transform.position : controller.Target.position;
        var fwdPoints = new List<Vector3>();

        foreach(var item in points)
        {

            var dir = targetPos - item;

            if(Vector3.Dot(targetFwd, dir.normalized) > 0)
            {

                fwdPoints.Add(item);

            }

        }

        if(fwdPoints.Count != 0)
        {

            controller.MoveController.Move(fwdPoints.GetRandomListObject());

        }

        controller.RemovingTarget();

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
    public LightMonsterTargetCastingTransition(FSM_Controller_Netcode<LigthMonsterStateType> controller, LigthMonsterStateType nextState) : base(controller, nextState)
    {
    }

    protected override bool CheckTransition()
    {

        controller.CheckTargetting();

        return controller.Target != null;

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

    private bool lightCasted;

    public LightMonsterLightTransition(FSM_Controller_Netcode<LigthMonsterStateType> controller, LigthMonsterStateType nextState) : base(controller, nextState)
    {


    }

    public override void EnterTransition()
    {

        lightCasted = false;
        controller.OnCastedEvent += HandleLightCasted;

    }

    private void HandleLightCasted()
    {

        lightCasted = true;

    }

    public override void ExitTransition()
    {

        controller.OnCastedEvent -= HandleLightCasted;

    }

    protected override bool CheckTransition()
    {

        return lightCasted;

    }

}

#endregion

[RequireComponent(typeof(MonsterController))]
public class LightMonsterFSM : FSM_Controller_Netcode<LigthMonsterStateType>, ILightCastable
{

    [SerializeField] private LayerMask targetMask;
    [SerializeField] private LayerMask obstacleMask;
    [SerializeField] private float angle;

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
        var runAway = new LightMonsterRunAwayState(this);

        var goRun = new LightMonsterLightTransition(this, LigthMonsterStateType.RunAway);
        var goChase = new LightMonsterTargetCastingTransition(this, LigthMonsterStateType.Chase);
        var goPatrol = new LightMonsterMoveEndTransition(this, LigthMonsterStateType.Patrol);

        patrol.AddTransition(goRun);
        patrol.AddTransition(goChase);

        chase.AddTransition(goRun);

        runAway.AddTransition(goPatrol);

        AddState(patrol, LigthMonsterStateType.Patrol);
        AddState(chase, LigthMonsterStateType.Chase);
        AddState(runAway, LigthMonsterStateType.RunAway);

    }

    protected override void Update()
    {

        if (!IsServer) return;

        base.Update();

    }

    private void InitComponent()
    {

        MoveController = GetComponent<MonsterController>(); 

    }

    public void Casting()
    {

        Debug.Log("OhNO!");
        OnCastedEvent?.Invoke();

    }

    public void CheckTargetting()
    {

        var target = MoveController.ViewingAndGetClosest(10, angle, targetMask, obstacleMask);

        if(target != null)
        {

            Target = target.transform;

        }

    }

    public void RemovingTarget()
    {

        Target = null;

    }

}
