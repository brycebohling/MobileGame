using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIBasicAttack : AIBase
{
    public AIStates.States[] BlockingActionStates;

    [SerializeField] float attackRadius;
    [SerializeField] LayerMask targetLayer;
    [SerializeField] float damage;
    [SerializeField] float knockBackForce;




    protected override void Awake()
    {
        base.Awake();
    }

    void Update()
    {           
        if (!IsActionAuth(BlockingActionStates)) return;

        HandleAction();
    }

    protected override void OnActionActivate()
    {
        base.OnActionActivate();
    }

    protected override void OnActionDeactivate()
    {
        base.OnActionDeactivate();
    }

    protected override void HandleAction()
    {
        if (!IsPlayerInRange(attackRadius, targetLayer)) return;
        
        Attack();
    }

    private void Attack()
    {
        
    }
}
