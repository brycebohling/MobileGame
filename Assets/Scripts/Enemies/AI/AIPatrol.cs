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
    [SerializeField] AnimationClip walkAnim;

    Vector2 startPos;
    float tbNextMovement;
    float timeTillReroute;
    bool isMovingToPoint;
    Vector2 movePoint;

    Vector2 currentVelocity;

    
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

    void FixedUpdate()
    {
        if (!IsActionAuth(BlockingActionStates)) return;

        if (_aIStatesScript.State == AIStates.States.Patrolling)
        {
            _rb.velocity = currentVelocity;
        }
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

        StopAnimation(_animator);
    }

    protected override void HandleAction()
    {
        if (isMovingToPoint)
        {
            StartAnimation(_animator, walkAnim);

            if (Vector2.Distance(transform.position, movePoint) < requiredDistanceFromPoint)
            {
                currentVelocity = Vector2.zero;

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
        currentVelocity = moveDir * patrolSpeed;

        isMovingToPoint = true;
        _aIStatesScript.State = AIStates.States.Patrolling;
        timeTillReroute = 0;
    }

    protected override void StartAnimation(Animator anim, AnimationClip animClip)
    {
        base.StartAnimation(anim, animClip);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(startPos, patrolRadius);
    }
}
