using System;
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
    [SerializeField] float dashCooldownTime;
    [SerializeField] float targetingRadius;
    [SerializeField] LayerMask playerLayer;
    [SerializeField] LayerMask obstacleLayer;
    [SerializeField] AnimationClip preDashAnim;
    [SerializeField] AnimationClip dashAnim;

    bool isActivated;
    bool isDashing;
    bool isPreDash;
    float preDashCounter;
    float dashCounter;
    float dashCooldownCounter;


    protected override void Awake()
    {
        base.Awake();
    }

    void Update()
    {
        UpdateTimers();
        
        if (!IsActionAuth(BlockingActionStates)) 
        {
            if (isActivated)
            {
                OnActionCancel();
            }

            return;
        }

        HandleAction();
    }

    private void UpdateTimers()
    {
        if (isPreDash)
        {
            preDashCounter += Time.deltaTime;
        }
        
        if (isDashing)
        {
            dashCounter += Time.deltaTime;
        }

        if (!isPreDash && !isDashing)
        {
            dashCooldownCounter += Time.deltaTime;
        }
    }

    protected override void OnActionActivate()
    {
        isActivated = true;
        _aiPathScript.canMove = false;
        _rb.velocity = Vector2.zero;
        _aIStatesScript.State = AIStates.States.Dashing;
    }

    protected override void OnActionDeactivate()
    {
        isActivated = false;
        isPreDash = false;
        isDashing = false;
        preDashCounter = 0;
        dashCounter = 0;
        dashCooldownCounter = 0;

        _aIStatesScript.State = AIStates.States.Idle;

        StopAnimation(_animator);
    }

    protected override void OnActionCancel()
    {
        isActivated = false;
        isPreDash = false;
        isDashing = false;
        preDashCounter = 0;
        dashCounter = 0;
        dashCooldownCounter = 0;
    }

    protected override void HandleAction()
    {
        if (isDashing)
        {
            if (dashCounter >= dashTime)
            {
                OnActionDeactivate();
                return;
            }
        }

        if ((!IsTargetInRange(targetingRadius, playerLayer) && _aIStatesScript.State != AIStates.States.Dashing) 
            || dashCooldownCounter < dashCooldownTime || isDashing) return;

        if (!isPreDash)
        {
            OnActionActivate();
            PreDash();

        } else if (preDashCounter >= preDashTime)
        {
            Dash();
        }
    }

    private void PreDash()
    {
        isPreDash = true;

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
