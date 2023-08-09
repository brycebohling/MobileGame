using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIChase : AIBase
{
    public AIStates.States[] BlockingActionStates;
    [SerializeField] float chaseSpeed;
    [SerializeField] float targetingRadius;
    [SerializeField] float rotationSpeed;
    [SerializeField] LayerMask playerLayer;


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

    protected override void HandleAction()
    {
        RaycastHit2D[] hitPlayers = Physics2D.CircleCastAll(transform.position, targetingRadius, Vector2.zero, 0, playerLayer);

        if (hitPlayers.Length != 0)
        {
            float closestPlayerDistance = 10000;
            Vector2 closestPlayerPos = Vector2.zero;

            foreach (RaycastHit2D hitPlayer in hitPlayers)
            {
                if (Vector2.Distance(transform.position, hitPlayer.transform.position) < closestPlayerDistance)
                {
                    closestPlayerDistance = Vector2.Distance(transform.position, hitPlayer.transform.position);
                    closestPlayerPos = hitPlayer.transform.position;
                }
            }

            MoveToClosestPlayer(closestPlayerPos);

            _aIStatesScript.State = AIStates.States.Chasing;

        } else if (_aIStatesScript.State == AIStates.States.Chasing)
        {
            _aIStatesScript.State = AIStates.States.Idle;
        }
    }

    private void MoveToClosestPlayer(Vector2 closestPlayer)
    {
        Vector2 moveDir = (closestPlayer - (Vector2)transform.position).normalized;
        _rb.velocity = moveDir * chaseSpeed;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, targetingRadius);
    }
}
