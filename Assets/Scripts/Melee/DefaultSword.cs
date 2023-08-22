using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DefaultSword : PlayerMeleeBase
{
    [Header("Blocking States")]
    public PlayerStates.States[] BlockingActionStates;

    [Header("Attack")]
    [SerializeField] Transform attackPoint;
    [SerializeField] float dmg;
    [SerializeField] float knockBackForce;
    [SerializeField] float attackRadius;
    [SerializeField] LayerMask enemyLayer;

    [Header("Animations")]
    InputAction attackkeys;
    int lastAttackAnimIndex = 0;


    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Start()
    {
        base.Start();
    }

    private void OnEnable()
    {
        attackkeys = _inputManager.Player.Fire;
        attackkeys.Enable();
        attackkeys.performed += AttackPressed;
    }

    private void OnDisable()
    {
        attackkeys.Disable();
    }

    private void AttackPressed(InputAction.CallbackContext context)
    {
        if (!IsActionAuth(BlockingActionStates)) return;
        
        Attack();
    }

    private void Attack()
    {
        foreach (AnimationClip anim in _attackAnimList)
        {
            if (Helpers.IsAnimationPlaying(_meleeAnimator, anim.name))
            {
                return;
            }
        }

        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.transform.position, attackRadius, enemyLayer);

        foreach (Collider2D hitEnemy in hitEnemies)
        {
            hitEnemy.GetComponent<Health>().DamageObject(dmg, knockBackForce, _playerTransform.position);
        }

        Helpers.ChangeAnimationState(_meleeAnimator, _attackAnimList[lastAttackAnimIndex].name);

        if (_attackAnimList.Count <= lastAttackAnimIndex + 1)
        {
            lastAttackAnimIndex = 0;

        } else
        {
            lastAttackAnimIndex++;
        }
        
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(attackPoint.position, attackRadius);
    }
}
