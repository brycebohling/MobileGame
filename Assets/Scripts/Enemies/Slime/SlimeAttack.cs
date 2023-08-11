using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(AIStates))]
public class SlimeAttack : AIBase
{
    public AIStates.States[] BlockingActionStates;
    [SerializeField] float attackSpeed;
    [SerializeField] float targetingRadius;
    [SerializeField] LayerMask playerLayer;

    Vector2 currentVelocity;


    protected override void Start()
    {
        base.Start();
    }

    private void Update()
    {
        if (!IsActionAuth(BlockingActionStates)) return;
        
        HandleAction();
    }

    void FixedUpdate()
    {
        if (!IsActionAuth(BlockingActionStates)) return;

        if (_aIStatesScript.State == AIStates.States.Attacking)
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
            _aIStatesScript.State = AIStates.States.Attacking;

        } else if (_aIStatesScript.State == AIStates.States.Attacking)
        {
            _aIStatesScript.State = AIStates.States.Idle;
        }
    }

    private void MoveToClosestPlayer(Vector2 closestPlayer)
    {
        Vector2 moveDir = (closestPlayer - (Vector2)transform.position).normalized;
        currentVelocity = moveDir * attackSpeed;
    }

    protected override bool IsPlayerInRange(float attackRadius, LayerMask playerLayer)
    {
        return base.IsPlayerInRange(attackRadius, playerLayer);
    }

    protected override Vector2 FindClosesetPlayerInRange(float attackRadius, LayerMask playerLayer)
    {
        return base.FindClosesetPlayerInRange(attackRadius, playerLayer);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, targetingRadius);
    }
}