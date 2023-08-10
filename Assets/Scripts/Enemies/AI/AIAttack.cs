using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(IAttacker))]
public class AIAttack : AIBase
{
    public AIStates.States[] BlockingActionStates;
    
    [SerializeField] float dmg;
    [SerializeField] float tbAttacks;
    [SerializeField] float attackRadius;
    [SerializeField] LayerMask playerLayer;

    IAttacker attackScript;


    protected override void Start()
    {
        base.Start();

        attackScript = gameObject.GetComponent<IAttacker>();
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
        RaycastHit2D[] hitPlayers = Physics2D.CircleCastAll(transform.position, attackRadius, Vector2.zero, 0, playerLayer);

        if (hitPlayers.Length != 0)
        {
            Vector2 closestPlayerPos = hitPlayers[0].transform.position - transform.position;
            
            foreach (RaycastHit2D hitPlayer in hitPlayers)
            {
                if (((Vector2)hitPlayer.transform.position - (Vector2)transform.position).sqrMagnitude < closestPlayerPos.sqrMagnitude)
                {
                    closestPlayerPos = hitPlayer.transform.position;
                }
            }

            // AttackPlayer(closestPlayerPos);
        }
    }

    private void AttackPlayer(Vector2 closestPlayer)
    {
        attackScript.StartAttack(closestPlayer);
    }

    private void EndAttackPlayer(Vector2 closestPlayer)
    {
        _aIStatesScript.State = AIStates.States.Idle;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, attackRadius);
    }
}
