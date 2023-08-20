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
    [SerializeField] AnimationClip walkAnim;

    bool isActivated;


    protected override void Start()
    {
        base.Start();
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
        isActivated = true;
        StartAnimation(_animator, walkAnim);
    }

    protected override void OnActionDeactivate()
    {
        isActivated = false;
        StopAnimation(_animator);
    }

    protected override void HandleAction()
    {
        if (IsPlayerInRange(targetingRadius, playerLayer))
        {
            MoveToClosestPlayer(FindClosesetPlayerInRange(targetingRadius, playerLayer));
            _aIStatesScript.State = AIStates.States.Chasing;

            if (!isActivated)
            {
                OnActionActivate();
            }

        } else if (_aIStatesScript.State == AIStates.States.Chasing)
        {
            _aIStatesScript.State = AIStates.States.Idle;

            OnActionDeactivate();
        }
    }

    protected override bool IsPlayerInRange(float attackRadius, LayerMask playerLayer)
    {
        return base.IsPlayerInRange(attackRadius, playerLayer);
    }

    protected override Vector2 FindClosesetPlayerInRange(float attackRadius, LayerMask playerLayer)
    {
        return base.FindClosesetPlayerInRange(attackRadius, playerLayer);
    }

    private void MoveToClosestPlayer(Vector2 closestPlayer)
    {
        Vector2 moveDir = (closestPlayer - (Vector2)transform.position).normalized;

        _aiPathScript.destination = closestPlayer;
        _aiPathScript.maxSpeed = chaseSpeed;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, targetingRadius);
    }
}