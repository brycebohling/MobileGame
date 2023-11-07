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
    [SerializeField] float attackPerSecond;
    [SerializeField] float projectileSpeed;
    [SerializeField] float knockBackForce;
    float tbAttacksCounter;
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
        tbAttacksCounter += Time.deltaTime;

        if (attackKeys.IsPressed())
        {
            if (IsActionAuth(BlockingActionStates))
            {
                Attack();
            }
            
        } else
        {
            Helpers.ChangeAnimationState(_animator, emptyAnim.name, 1);
        }
    }

    protected override bool IsActionAuth(PlayerStates.States[] blockingActionStates)
    {
        if (!base.IsActionAuth(blockingActionStates)) return false;

        if (tbAttacksCounter < attackPerSecond) return false;

        return true;
    }

    private void Attack()
    {
        Transform projectile = Instantiate(projectilePrefab, attackPoint.position, Quaternion.identity);
        
        Vector2 mouseDirection = attackPoint.position - transform.position;
        projectile.GetComponent<DefaultWandProjectile>().Spawn(mouseDirection, projectileSpeed, dmg, knockBackForce);

        Helpers.ChangeAnimationState(_animator, fireAnim.name, 1);

        tbAttacksCounter = 0;
    }
}
