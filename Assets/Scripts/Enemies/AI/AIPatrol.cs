using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class AIPatrol : AIBase
{
    public AIStates.States[] BlockingActionStates;
    [SerializeField] float patrolRadius;
    [SerializeField] float patrolSpeed;
    [SerializeField] float tbMovementMin;
    [SerializeField] float tbMovementMax;
    [SerializeField] float moveTimeBeforeReroute;
    [SerializeField] float requiredDistanceFromPoint;

    Vector2 startPos;
    float tbNextMovement;
    float timeTillReroute;
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
        tbNextMovement = 0;
        timeTillReroute = 0;
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
                tbNextMovement = Random.Range(tbMovementMin, tbMovementMax);
            } else
            {
                timeTillReroute += Time.deltaTime;

                if (timeTillReroute >= moveTimeBeforeReroute)
                {
                    MoveToNextPoint(GetMovePos());
                }
            }

        } else
        {
            if (tbNextMovement <= 0)
            {
                MoveToNextPoint(GetMovePos());

            } else
            {
                tbNextMovement -= Time.deltaTime;

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
        _aIStatesScript.State = AIStates.States.Patrolling;
        timeTillReroute = 0;
    }


    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(startPos, patrolRadius);
    }
}
