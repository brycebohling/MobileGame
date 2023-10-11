using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class AIDash : AIBase
{
    public AIStates.States[] BlockingActionStates;
    [SerializeField] float dashSpeed;
    [SerializeField] float preDashTime;
    [SerializeField] float dashTime;
    [SerializeField] float dashCooldown;
    [SerializeField] float targetingRadius;
    [SerializeField] LayerMask playerLayer;
    [SerializeField] AnimationClip preDashAnim;
    [SerializeField] AnimationClip dashAnim;

    bool isDashing;
    bool isPreDash;
    float preDashCounter;
    float dashCounter;
    float dashCooldownCounter;

    bool isActionCanceled;


    protected override void Awake()
    {
        base.Awake();
    }

    void Update()
    {
        UpdateTimers();
        
        if (!IsActionAuth(BlockingActionStates)) 
        {
            if (!isActionCanceled)
            {
                OnActionDeactivate();
            }

            return;
        }

        HandleAction();
    }

    private void UpdateTimers()
    {
        if (isPreDash)
        {
            preDashCounter -= Time.deltaTime;
        }
        
        if (isDashing)
        {
            dashCounter -= Time.deltaTime;
        }

        if (!isPreDash && !isDashing)
        {
            dashCooldownCounter -= Time.deltaTime;
        }
    }

    protected override void OnActionActivate()
    {
        base.OnActionActivate();

        preDashCounter = preDashTime;
        dashCounter = dashTime;
        

        isPreDash = true;

        _aIStatesScript.State = AIStates.States.Dashing;
    }

    protected override void OnActionDeactivate()
    {
        isPreDash = false;
        isDashing = false;
        preDashCounter = preDashTime;
        dashCounter = dashTime;
        dashCooldownCounter = dashCooldown;

        _aIStatesScript.State = AIStates.States.Idle;

        StopAnimation(_animator);
    }

    protected override void HandleAction()
    {
        if (_aIStatesScript.State == AIStates.States.Dashing)
        {
            if (dashCounter <= 0 || !IsPlayerInRange(targetingRadius, playerLayer))
            {
                OnActionDeactivate();
                return;
            }
        }

        if (!IsPlayerInRange(targetingRadius, playerLayer) || dashCooldownCounter > 0 || isDashing) return;

        if (!isPreDash)
        {
            OnActionActivate();
            PreDash();

        } else if (preDashCounter <= 0)
        {
            Dash();
        }
    }

    private void PreDash()
    {
        _aiPathScript.canMove = false;

        StartAnimation(_animator, preDashAnim);
    }

    private void Dash()
    {
        Vector2 direction = (GameManager.Gm.playerTransfrom.position - transform.position).normalized;
        _rb.velocity = direction * dashSpeed;

        isDashing = true;
        isPreDash = false;
        
        StartAnimation(_animator, dashAnim);
    }
}
