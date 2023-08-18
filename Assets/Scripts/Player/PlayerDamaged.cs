using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Health))]
public class PlayerDamaged : CharacterBase
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
            _rb.velocity = Vector2.zero;
            _characterStatesScript.State = CharacterStates.States.Idle;
        }
    }

    protected override bool IsActionAuth(CharacterStates.States[] blockingActionStates)
    {
        if (_characterStatesScript.State == CharacterStates.States.Damaged)
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

        _characterStatesScript.State = CharacterStates.States.Damaged;
    }

    private void KnockBack(float knockBackForce, Vector2 senderPos)
    {
        Vector2 dir = ((Vector2)transform.position - senderPos).normalized;
        _rb.AddForce(dir * knockBackForce, ForceMode2D.Impulse);

        _characterStatesScript.State = CharacterStates.States.Damaged;
    }
}
