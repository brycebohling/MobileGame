using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIChase : AIBase
{
    public AIStates.States[] BlockingActionStates;
    [SerializeField] float chaseSpeed;
    [SerializeField] float targetingRadius;
    [SerializeField] LayerMask playerLayer;

    Vector2 currentVelocity;


    protected override void Start()
    {
        base.Start();
    }

    void Update()
    {
        if (!IsActionAuth(BlockingActionStates)) return;

        HandleAction();
    }

    void FixedUpdate()
    {
        if (!IsActionAuth(BlockingActionStates)) return;
        
        if (_aIStatesScript.State == AIStates.States.Chasing)
        {
            _rb.velocity = currentVelocity;
        }
    }

    protected override bool IsActionAuth(AIStates.States[] blockingActionStates)
    {
        return base.IsActionAuth(blockingActionStates);
    }

    protected override void HandleAction()
    {
        if (IsPlayerInRange(targetingRadius, playerLayer))
        {
            MoveToClosestPlayer(FindClosesetPlayerInRange(targetingRadius, playerLayer));
            _aIStatesScript.State = AIStates.States.Chasing;

        } else if (_aIStatesScript.State == AIStates.States.Chasing)
        {
            _aIStatesScript.State = AIStates.States.Idle;
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
        currentVelocity = moveDir * chaseSpeed;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, targetingRadius);
    }
}