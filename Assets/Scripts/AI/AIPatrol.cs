using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class AIPatrol : AIBase
{
    public AIStates.States[] BlockingActionStates;
    [SerializeField] float patrolRadius;
    [SerializeField] float patrolSpeed;
    [SerializeField] float timeBetweenMovementMin;
    [SerializeField] float timeBetweenMovementMax;
    [SerializeField] float requiredDistanceFromPoint;

    Vector2 startPos;
    float timeBetweenNextMovement;
    bool isMovingToPoint;
    Vector2 movePoint;

    

    protected override void Start()
    {
        base.Start();

        startPos = transform.position;
    }

    void Update()
    {
        if (!IsActionAuth(BlockingActionStates))
        {
            RestAction();
            return;
        } 

        HandleAction();
    }

    protected override bool IsActionAuth(AIStates.States[] blockingActionStates)
    {
        return base.IsActionAuth(blockingActionStates);
    }

    protected override void RestAction()
    {
        timeBetweenNextMovement = 0;
        isMovingToPoint = false;
        movePoint = Vector2.zero;
    }

    protected override void HandleAction()
    {
        if (isMovingToPoint)
        {
            if (Vector2.Distance(transform.position, movePoint) < requiredDistanceFromPoint)
            {
                _rb.velocity = Vector2.zero;

                isMovingToPoint = false;
                timeBetweenNextMovement = Random.Range(timeBetweenMovementMin, timeBetweenMovementMax);
            }
        } else
        {
            if (timeBetweenNextMovement <= 0)
            {
                MoveToNextPoint(GetMovePos());
                _aIStatesScript.State = AIStates.States.Patrolling;
                
            } else
            {
                timeBetweenNextMovement -= Time.deltaTime;

                _aIStatesScript.State = AIStates.States.Idle;
            }
        }
    }

    private Vector2 GetMovePos()
    {
        movePoint = startPos + Random.insideUnitCircle * patrolRadius;
        return movePoint;
    }

    private void MoveToNextPoint(Vector2 nextPoint)
    {
        Vector2 moveDir = (nextPoint - (Vector2)transform.position).normalized;
        _rb.velocity = moveDir * patrolSpeed;

        isMovingToPoint = true;
    }


    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(startPos, patrolRadius);
    }
}
