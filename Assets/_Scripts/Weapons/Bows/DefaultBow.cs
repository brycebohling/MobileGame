using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DefaultBow : PlayerWeaponBase
{
    [Header("Blocking States")]
    public PlayerStates.States[] BlockingActionStates;

    [Header("Attack")]
    [SerializeField] Transform arrowPrefab;
    [SerializeField] Transform attackPoint;
    [SerializeField] float dmg;
    [SerializeField] float arrowSpeed;
    [SerializeField] float knockBackForce;
    [SerializeField] LayerMask enemyLayerMask;
    InputAction attackKeys;

    [Header("Animations")]
    [SerializeField] AnimationClip drawBackArrow;
    [SerializeField] AnimationClip releaseBow;


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
        if (!Helpers.IsAnimationPlaying(_animator, releaseBow.name))
        {
            Helpers.ChangeAnimationState(_animator, drawBackArrow.name);
        }
    }

    private void AttackPressed(InputAction.CallbackContext context)
    {
        if (!IsActionAuth(BlockingActionStates)) return;
        
        if (Helpers.IsAnimationPlaying(_animator, drawBackArrow.name) || Helpers.IsAnimationPlaying(_animator, releaseBow.name)) return;

        Attack();
    }

    private void Attack()
    {
        foreach (AnimationClip anim in _attackAnimList)
        {
            if (Helpers.IsAnimationPlaying(_animator, anim.name)) return;
        }

        Transform arrow = Instantiate(arrowPrefab, attackPoint.position, Quaternion.identity);
        
        Vector2 mouseDirection = attackPoint.position - transform.position;
        arrow.GetComponent<DefaultArrow>().Initialize(mouseDirection, arrowSpeed, dmg, knockBackForce, enemyLayerMask);

        Helpers.ChangeAnimationState(_animator, releaseBow.name);        
    }
}
