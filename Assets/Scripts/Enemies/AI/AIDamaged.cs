using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Health))]
public class AIDamaged : AIBase
{
    [SerializeField] float damagedTime;
    float damagedTimer = 0;


    protected override void Awake()
    {
        base.Start();
    }

    protected override void OnEnable() 
    {
        _healthScript.OnDamaged += Damaged;
    }

    protected override void OnDisable() 
    {
        _healthScript.OnDamaged -= Damaged;
    }

    private void Update() 
    {
        if (!IsActionAuth(null)) return;
        
        damagedTimer += Time.deltaTime;
        if (damagedTimer >= damagedTime)
        {
            _aiPathScript.canMove = true;
            _aIStatesScript.State = AIStates.States.Idle;
        }
    }

    protected override bool IsActionAuth(AIStates.States[] blockingActionStates)
    {
        if (_aIStatesScript.State == AIStates.States.Damaged)
        {
            return true;
        } else
        {
            return false;
        }
    }

    private void Damaged(float knockBackForce, Vector2 senderPos)
    {
        KnockBack(knockBackForce, senderPos);
        damagedTimer = 0;

        _aIStatesScript.State = AIStates.States.Damaged;
    }

    private void KnockBack(float knockBackForce, Vector2 senderPos)
    {
        Vector2 dir = ((Vector2)transform.position - senderPos).normalized;
        _rb.AddForce(dir * knockBackForce, ForceMode2D.Impulse);

        _aiPathScript.canMove = false;
        _aIStatesScript.State = AIStates.States.Damaged;
    }
}