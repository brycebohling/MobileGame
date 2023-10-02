using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class DefaultWand : PlayerWeaponBase
{
    [Header("Blocking States")]
    public PlayerStates.States[] BlockingActionStates;

    [Header("Attack")]
    [SerializeField] Transform projectilePrefab;
    [SerializeField] Transform attackPoint;
    [SerializeField] float dmg;
    [SerializeField] float attackSpeed;
    float tbAttacks;
    [SerializeField] float projectileSpeed;
    [SerializeField] float knockBackForce;
    [SerializeField] LayerMask enemyLayerMask;
    InputAction attackKeys;

    [Header("Animations")]
    [SerializeField] AnimationClip emptyAnim;
    [SerializeField] AnimationClip fireAnim;


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
    }

    private void OnDisable()
    {
        attackKeys.Disable();
    }

    private void Update()
    {
        tbAttacks += Time.deltaTime;

        if (attackKeys.IsPressed())
        {
            if (IsActionAuth(BlockingActionStates))
            {
                Attack();
            }
            
        } else
        {
            Helpers.ChangeAnimationState(_animator, emptyAnim.name);
        }
    }

    protected override bool IsActionAuth(PlayerStates.States[] blockingActionStates)
    {
        if (!base.IsActionAuth(blockingActionStates)) return false;

        if (tbAttacks < attackSpeed) return false;

        return true;
    }

    private void Attack()
    {
        Transform projectile = Instantiate(projectilePrefab, attackPoint.position, Quaternion.identity);
        
        Vector2 mouseDirection = attackPoint.position - transform.position;
        projectile.GetComponent<DefaultWandProjectile>().Initialize(mouseDirection, projectileSpeed, dmg, knockBackForce, enemyLayerMask);

        Helpers.ChangeAnimationState(_animator, fireAnim.name);

        tbAttacks = 0;
    }
}
