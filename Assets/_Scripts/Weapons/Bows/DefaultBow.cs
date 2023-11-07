using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class DefaultBow : PlayerWeaponBase
{
    [Header("Blocking States")]
    public PlayerStates.States[] BlockingActionStates;

    [Header("Attack")]
    [SerializeField] Transform arrowPrefab;
    [SerializeField] Transform attackPoint;
    [SerializeField] float dmg;
    [SerializeField] float attackPerSecond;
    [SerializeField] float arrowSpeed;
    [SerializeField] float knockBackForce;
    InputAction attackKeys;

    [Header("Animations")]
    [SerializeField] AnimationClip drawBackArrow;
    [SerializeField] AnimationClip releaseBow;

    bool bowDrawnBack;
    float animSpeed;


    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Start()
    {
        float totalAnimLength = drawBackArrow.length + releaseBow.length;

        animSpeed = totalAnimLength / attackPerSecond;
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

    private void LateUpdate()
    {
        if (!bowDrawnBack && !Helpers.IsAnimationPlaying(_animator, releaseBow.name))
        {
            bowDrawnBack = true;

            Helpers.ChangeAnimationState(_animator, drawBackArrow.name, animSpeed);
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
        Transform arrow = Instantiate(arrowPrefab, attackPoint.position, Quaternion.identity);
        
        Vector2 mouseDirection = attackPoint.position - transform.position;
        arrow.GetComponent<DefaultArrow>().Spawn(mouseDirection, arrowSpeed, dmg, knockBackForce);
        
        bowDrawnBack = false;

        Helpers.ChangeAnimationState(_animator, releaseBow.name, animSpeed);
    }
}
