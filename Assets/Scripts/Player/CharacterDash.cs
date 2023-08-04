using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterMovement))]
public class CharacterDash : CharacterBase
{
    [Header("Blocking States")]
    public CharacterStates.MovementStates[] BlockingMovementStates;
    public CharacterStates.CharacterConditions[] BlockingConditionStates;
    
    [SerializeField] float dashSpeedMultiplayer;
    [SerializeField] float dashDuration;
    [SerializeField] float dashCooldown;
    [SerializeField] bool invisibleInDash;

    InputAction dashKeys;

    bool isDashing;
    float dashingTimer;
    float dashCooldownTimer;
    Vector2 velocityBeforeDash;


    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Start()
    {
        base.Start();
    }

    private void Update()
    {
        if (isDashing)
        {
            _rb.velocity = velocityBeforeDash * dashSpeedMultiplayer * Time.deltaTime;

            dashingTimer += Time.deltaTime;
            if (dashingTimer >= dashDuration)
            {
                ResetAbility();
            }
        } else
        {
            dashCooldownTimer += Time.deltaTime;
        }
    }

    protected override void OnEnable()
    {
        dashKeys = _inputManager.Player.Dash;
        dashKeys.Enable();
        dashKeys.performed += DashPressed;
    }

    protected override void OnDisable()
    {
        dashKeys.Disable();
    }

    private void DashPressed(InputAction.CallbackContext context)
    {
        ProcessAbility();
    }

    protected override void ProcessAbility()
    {
        Debug.Log("trying to dash");
        if (!isDashing && dashCooldownTimer >= dashCooldown)

        foreach (CharacterStates.MovementStates state in BlockingMovementStates)
        {
            if (state == _characterStatesScript._movementState)
            {
                return;
            }
            Debug.Log("dash");
            _characterStatesScript._movementState = CharacterStates.MovementStates.Dashing;
            velocityBeforeDash = _rb.velocity;
            isDashing = true;
        }
    }

    protected override void ResetAbility()
    {
        isDashing = false;
        dashCooldownTimer = 0;
        _characterStatesScript._movementState = CharacterStates.MovementStates.Idle;
    }
}
