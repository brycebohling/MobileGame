using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine;

[RequireComponent(typeof(AIPath))]
public class AIChase : AIBase
{
    public AIStates.States[] BlockingActionStates;
    [SerializeField] float chaseSpeed;
    [SerializeField] float targetingRadius;
    [SerializeField] LayerMask playerLayer;
    [SerializeField] float agroTime;
    [SerializeField] AnimationClip walkAnim;

    bool isActivated;
    float agroCounter;


    protected override void Awake()
    {
        base.Awake();
    }

    void Update()
    {
        if (!IsActionAuth(BlockingActionStates)) return;

        HandleAction();
    }

    protected override bool IsActionAuth(AIStates.States[] blockingActionStates)
    {
        return base.IsActionAuth(blockingActionStates);
    }

    protected override void OnActionActivate()
    {
        base.OnActionActivate();

        isActivated = true;

        _aIStatesScript.State = AIStates.States.Chasing;

        StartAnimation(_animator, walkAnim);
    }

    protected override void OnActionDeactivate()
    {
        isActivated = false;
        StopAnimation(_animator);

        _aIStatesScript.State = AIStates.States.Idle;
    }

    protected override void HandleAction()
    {
        if (agroCounter > 0)
        {
            if (!isActivated)
            {
                OnActionActivate();
            }

            ChaseClosestPlayer();

        } else
        {
            if (IsPlayerInRange(targetingRadius, playerLayer))
            {
                agroCounter = agroTime;

            } else if (_aIStatesScript.State == AIStates.States.Chasing)
            {
                OnActionDeactivate();
            }
        }
    }

    public void Damaged()
    {
        agroCounter = agroTime;
    }

    private void ChaseClosestPlayer()
    {
        _aiPathScript.destination = GameManager.Gm.playerTransfrom.position;
        _aiPathScript.maxSpeed = chaseSpeed;

        agroCounter -= Time.deltaTime;
    }
}