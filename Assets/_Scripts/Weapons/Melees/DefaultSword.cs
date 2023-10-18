using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DefaultSword : PlayerWeaponBase
{
    [Header("Blocking States")]
    public PlayerStates.States[] BlockingActionStates;

    [Header("Attack")]
    [SerializeField] Transform attackPoint;
    [SerializeField] float dmg;
    [SerializeField] float knockBackForce;
    [SerializeField] float attackRadius;
    [SerializeField] LayerMask enemyLayer;

    InputAction attackKeys;
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
        attackKeys = _inputManager.Player.Fire;
        attackKeys.Enable();
        attackKeys.performed += AttackPressed;
    }

    private void OnDisable()
    {
        attackKeys.Disable();
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
            if (Helpers.IsAnimationPlaying(_animator, anim.name))
            {
                return;
            }
        }

        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.transform.position, attackRadius, enemyLayer);

        foreach (Collider2D hitEnemy in hitEnemies)
        {
            hitEnemy.GetComponent<Health>().Damage(dmg, knockBackForce, _playerTransform.position);
        }

        Helpers.ChangeAnimationState(_animator, _attackAnimList[lastAttackAnimIndex].name);

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
