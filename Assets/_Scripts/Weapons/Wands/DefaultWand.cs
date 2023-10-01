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
        attackKeys.performed += AttackPressed;
    }

    private void OnDisable()
    {
        attackKeys.Disable();
    }

    private void Update()
    {
        // if (!Helpers.IsAnimationPlaying(_animator, fireAnim.name))
        // {
        //     Helpers.ChangeAnimationState(_animator, fireAnim.name);
        // }
    }

    private void AttackPressed(InputAction.CallbackContext context)
    {
        if (!IsActionAuth(BlockingActionStates)) return;
        
        // if (Helpers.IsAnimationPlaying(_animator, drawBackArrow.name) || Helpers.IsAnimationPlaying(_animator, releaseBow.name)) return;

        Attack();
    }

    private void Attack()
    {
        Transform projectile = Instantiate(projectilePrefab, attackPoint.position, Quaternion.identity);
        
        Vector2 mouseDirection = attackPoint.position - transform.position;
        projectile.GetComponent<DefaultWandProjectile>().Initialize(mouseDirection, projectileSpeed, dmg, knockBackForce, enemyLayerMask);

        Helpers.ChangeAnimationState(_animator, fireAnim.name);        
    }
}
