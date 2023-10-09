using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine;

[RequireComponent(typeof(AIPath))]
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
    bool isActivated;

    
    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        if (!IsActionAuth(BlockingActionStates))
        {
            if (isActivated)
            {
                OnActionDeactivate();
            }
            
            return;
        } 

        HandleAction();
        isActivated = true;
    }

    protected override bool IsActionAuth(AIStates.States[] blockingActionStates)
    {
        return base.IsActionAuth(blockingActionStates);
    }

    protected override void OnActionDeactivate()
    {
        isActivated = false;

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
                StopAnimation(_animator);
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
        _aiPathScript.destination = nextPoint;
        _aiPathScript.maxSpeed = patrolSpeed;
        
        isMovingToPoint = true;
        _aIStatesScript.State = AIStates.States.Patrolling;
        timeTillReroute = 0;

        StartAnimation(_animator, walkAnim);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(startPos, patrolRadius);
    }
}
