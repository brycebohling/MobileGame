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

    protected override void Start()
    {
        base.Start();
    }

    private void Update()
    {
        if (IsActionAuth(BlockingActionStates))
        {
            HandleAction();
        }
    }

    protected override bool IsActionAuth(AIStates.States[] blockingActionStates)
    {
        return base.IsActionAuth(blockingActionStates);
    } 

    protected override void HandleAction()
    {
        
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, targetingRadius);
    }
}