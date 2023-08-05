using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterDash : CharacterBase
{
    [Header("Blocking States")]
    public CharacterStates.MovementStates[] BlockingMovementStates;
    public CharacterStates.CharacterConditions[] BlockingConditionStates;

    [Header("Dash")]
    [SerializeField] float dashSpeedMultiplayer;
    [SerializeField] float dashDuration;
    [SerializeField] float dashCooldown;

    [Header("Particles")]
    [SerializeField] List<ParticleSystem> dashParticales;

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

    private void FixedUpdate()
    {
        Dash();
        ProcessCooldowns();
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
        if (!isDashing && dashCooldownTimer >= dashCooldown)
        {
            foreach (CharacterStates.MovementStates state in BlockingMovementStates)
            {
                if (state == _characterStatesScript._movementState)
                {
                    return;
                }
            }

            AbilityActivate();
        }
    }

    protected override void AbilityActivate()
    {
        _characterStatesScript._movementState = CharacterStates.MovementStates.Dashing;
        velocityBeforeDash = _rb.velocity;
        isDashing = true;

        StartParticles(dashParticales);
    }

    protected override void AbilityDeactivate()
    {
        _characterStatesScript._movementState = CharacterStates.MovementStates.Idle;
        isDashing = false;
        dashingTimer = 0;
        dashCooldownTimer = 0;
        
        StopParticles(dashParticales);
    }

    private void Dash()
    {
        if (isDashing)
        {
            _rb.velocity = velocityBeforeDash * dashSpeedMultiplayer;
    
            dashingTimer += Time.deltaTime;
            if (dashingTimer >= dashDuration)
            {
                AbilityDeactivate();
            }
        }
    }

    protected override void ProcessCooldowns()
    {
        dashCooldownTimer += Time.deltaTime;
    }

    protected override void StartParticles(List<ParticleSystem> particleList)
    {
        base.StartParticles(particleList);
    }

    protected override void StopParticles(List<ParticleSystem> particleList)
    {
        base.StartParticles(particleList);
    }
}
