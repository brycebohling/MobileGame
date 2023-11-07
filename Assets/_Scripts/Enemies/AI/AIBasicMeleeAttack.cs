using System;
using System.Collections.Generic;
using UnityEngine;

public class AIBasicMeleeAttack : AIBase
{
    public AIStates.States[] BlockingActionStates;

    [SerializeField] float attackTargetingRadius;
    [SerializeField] LayerMask targetLayer;
    [SerializeField] float damage;
    [SerializeField] float knockBackForce;
    [SerializeField] float attackCooldownTime;

    [Header("HitBoxes")]
    [SerializeField] float hitBoxRadius;
    [SerializeField] float offsetFromCenter;

    [Header("Animations")]
    [SerializeField] AnimationClip[] attackAnims; 
    
    [Header("Gizmos")]
    [SerializeField] bool showAttackTargetingRadius;
    [SerializeField] bool showAttackRadius;

    bool isActivated;
    float attackCooldownCounter;
    bool isPlayingAttackAnim;
    Vector2 attackingDirection;


    protected override void Awake()
    {
        base.Awake();

        attackCooldownCounter = attackCooldownTime;
    }

    void Update()
    {
        UpdateTimers();

        if (!IsActionAuth(BlockingActionStates))
        {
            if (isActivated) OnActionCancel();

            return;
        }

        HandleAction();
    }

    protected override void OnActionActivate()
    {
        base.OnActionActivate();

        isActivated = true;
        _aiPathScript.canMove = false;
        _aIStatesScript.State = AIStates.States.Attacking;
    }

    protected override void OnActionDeactivate()
    {
        isActivated = false;
        _aIStatesScript.State = AIStates.States.Idle;
    }

    protected override void OnActionCancel()
    {
        isActivated = false;
    }

    private void UpdateTimers()
    {
        if (!isPlayingAttackAnim) 
        {
            attackCooldownCounter += Time.deltaTime;
        }
    }

    protected override void HandleAction()
    {
        foreach (AnimationClip animClip in attackAnims)
        {
            isPlayingAttackAnim = false;

            if (Helpers.IsAnimationPlaying(_animator, animClip.name))
            {
                isPlayingAttackAnim = true;
                break;
            }
        }

        if (!isActivated && IsTargetInRange(attackTargetingRadius, targetLayer))
        {
            OnActionActivate();

        } else if (!IsTargetInRange(attackTargetingRadius, targetLayer) && !isPlayingAttackAnim)
        {
            if (isActivated && _aIStatesScript.State == AIStates.States.Attacking) 
            {
                OnActionDeactivate();

            } else if (isActivated)
            {
                OnActionCancel();
            }

            return;
        }

        if (attackCooldownCounter < attackCooldownTime)
        {
            if (!isPlayingAttackAnim)
            {
                Helpers.ChangeAnimationState(_animator, _aIStatesScript.baseAnimationClip.name, 1);
            }

            return;

        }

        if (isPlayingAttackAnim) return;
        
        float angleToPlayer = Mathf.Atan2(GameManager.Gm.playerTransfrom.position.y - transform.position.y,
        GameManager.Gm.playerTransfrom.position.x - transform.position.x) * Mathf.Rad2Deg;

        _aiSpriteFlipper.FlipTowardsTarget(GameManager.Gm.playerTransfrom.position.x);

        if (Mathf.Abs(angleToPlayer) <= 45)
        {
            Helpers.ChangeAnimationState(_animator, attackAnims[(int)Helpers.Directions.Right].name, 1);
            attackingDirection = Vector2.right;

        } else if (Mathf.Abs(angleToPlayer) >= 135)
        {
            Helpers.ChangeAnimationState(_animator, attackAnims[(int)Helpers.Directions.Left].name, 1);
            attackingDirection = Vector2.left;

        } else if (angleToPlayer < 135 && angleToPlayer > 45)
        {
            Helpers.ChangeAnimationState(_animator, attackAnims[(int)Helpers.Directions.Up].name, 1);
            attackingDirection = Vector2.up;

        } else
        {
            Helpers.ChangeAnimationState(_animator, attackAnims[(int)Helpers.Directions.Down].name, 1);
            attackingDirection = Vector2.down;
        }   

        isPlayingAttackAnim = true;   
    }

    public void OnAttackFrame()
    {
        Vector2 attackPoint;
        attackPoint = (Vector2)transform.position + (attackingDirection * offsetFromCenter);

        Collider2D hitTarget = Physics2D.OverlapCircle(attackPoint, hitBoxRadius, targetLayer);

        if (hitTarget == null) return;
        
        if (hitTarget.TryGetComponent(out Health healthScript))
        {
            healthScript.Damage(damage, knockBackForce, transform.position);
        }

        attackCooldownCounter = 0;
    }

    private void OnDrawGizmos() 
    {
        if (showAttackTargetingRadius)
        {
            Gizmos.DrawWireSphere(transform.position, attackTargetingRadius);    
        }

        if (showAttackRadius)
        {
            Gizmos.DrawWireSphere((Vector2)transform.position + (Vector2.up * offsetFromCenter), hitBoxRadius);
            Gizmos.DrawWireSphere((Vector2)transform.position + (Vector2.down * offsetFromCenter), hitBoxRadius);
            Gizmos.DrawWireSphere((Vector2)transform.position + (Vector2.right * offsetFromCenter), hitBoxRadius);
            Gizmos.DrawWireSphere((Vector2)transform.position + (Vector2.left * offsetFromCenter), hitBoxRadius);
        }
    }
}
