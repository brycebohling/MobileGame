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
    [SerializeField] float attackPerSecond;
    [SerializeField] float knockBackForce;
    [SerializeField] Vector2 attackBoxCollider;
    [SerializeField] LayerMask targetLayer;
    
    PlayerWeaponMouseFollower playerWeaponMouseFollowerScript;
    InputAction attackKeys;
    int lastAttackAnimIndex = 0;


    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Start()
    {
        playerWeaponMouseFollowerScript = GameManager.Gm.playerTransfrom?.GetComponent<PlayerWeaponMouseFollower>();

        if (playerWeaponMouseFollowerScript == null)
        {
            Debug.LogWarning("playerWeaponMouseFollowerScript is null!");
        }
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
        
        HandleAction();
    }

    private void HandleAction()
    {
        foreach (AnimationClip anim in _attackAnimList)
        {
            if (Helpers.IsAnimationPlaying(_animator, anim.name))
            {
                return;
            }
        }

        PlayAttackAnim();

        if (_attackAnimList.Count <= lastAttackAnimIndex + 1)
        {
            lastAttackAnimIndex = 0;

        } else
        {
            lastAttackAnimIndex++;
        }
    }

    private void PlayAttackAnim()
    {
        float animSpeed = _attackAnimList[lastAttackAnimIndex].length / attackPerSecond;

        Helpers.ChangeAnimationState(_animator, _attackAnimList[lastAttackAnimIndex].name, animSpeed);
    }

    public void Attack()
    {
        Collider2D[] hitTargets = Physics2D.OverlapBoxAll(attackPoint.transform.position, attackBoxCollider, 
            playerWeaponMouseFollowerScript.weaponAngle, targetLayer);

        foreach (Collider2D hitTarget in hitTargets)
        {
            if (hitTarget.TryGetComponent(out Health healthScript))
            {
                healthScript.Damage(dmg, knockBackForce, _playerTransform.position);
            }
        }        
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(attackPoint.position, attackBoxCollider);
    }
}
