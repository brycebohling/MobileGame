using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Health))]
public class AIDamaged : AIBase
{
    public AIStates.States[] KeepActionStates;

    public UnityEvent OnDamaged;

    [SerializeField] float damagedTime;
    float damagedTimer = 0;

    [Header("Animations")]
    [SerializeField] AnimationClip damagedAnim;

    AIStates.States revertToState;
    bool isRevertingToState;


    protected override void Awake()
    {
        base.Awake();
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
        if (_aIStatesScript.State != AIStates.States.Damaged) return;
        
        damagedTimer += Time.deltaTime;

        if (damagedTimer >= damagedTime)
        {
            RestDamagedState();
        }
    }

    public void RestDamagedState()
    {
        _rb.velocity = Vector2.zero;

        if (isRevertingToState)
        {
            _aIStatesScript.State = revertToState;

            isRevertingToState = false;
            
        } else
        {
            _aiPathScript.canMove = true;

            _aIStatesScript.State = AIStates.States.Idle;
        }
    }

    private bool ShouldRevertToState(AIStates.States[] keepActionStates)
    {
        foreach (AIStates.States state in keepActionStates)
        {
            if (state == _aIStatesScript.State)
            {
                return true;
            }
        }
        
        return false;
    }

    private void Damaged(float dmg, float currentHealth, float knockBackForce, Vector2 senderPos)
    {
        KnockBack(knockBackForce, senderPos);
        damagedTimer = 0;

        StartAnimation(_animator, damagedAnim);

        if (ShouldRevertToState(KeepActionStates))
        {
            revertToState = _aIStatesScript.State;

            isRevertingToState = true;
        }

        _aIStatesScript.State = AIStates.States.Damaged;

        OnDamaged?.Invoke();
    }

    private void KnockBack(float knockBackForce, Vector2 senderPos)
    {
        Vector2 dir = ((Vector2)transform.position - senderPos).normalized;

        _rb.velocity = Vector2.zero;
        _rb.AddForce(dir * knockBackForce, ForceMode2D.Impulse);

        _aiPathScript.canMove = false;
    }
}   