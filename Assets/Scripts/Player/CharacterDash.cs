using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CharacterStates))]
public class CharacterDash : CharacterBase
{
    [Header("Blocking States")]
    public CharacterStates.States[] BlockingMovementStates;

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

        dashCooldownTimer = dashCooldown;
    }

    private void Update()
    {
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
        ProcessAbilityRequest();
    }

    protected override void ProcessAbilityRequest()
    {
        if (!isDashing && dashCooldownTimer >= dashCooldown)
        {
            foreach (CharacterStates.States state in BlockingMovementStates)
            {
                if (state == _characterStatesScript.State)
                {
                    return;
                }
            }

            OnAbilityActivate();
        }
    }

    protected override void OnAbilityActivate()
    {
        _characterStatesScript.State = CharacterStates.States.Dashing;
        velocityBeforeDash = _rb.velocity;
        isDashing = true;

        _rb.AddForce(velocityBeforeDash * dashSpeedMultiplayer, ForceMode2D.Impulse);

        StartParticles(dashParticales);
    }

    protected override void OnAbilityDeactivate()
    {
        _characterStatesScript.State = CharacterStates.States.Idle;
        isDashing = false;
        dashingTimer = 0;
        dashCooldownTimer = 0;
        
        StopParticles(dashParticales);
    }

    protected override void ProcessCooldowns()
    {
        if (isDashing)
        {
    
            dashingTimer += Time.deltaTime;
            if (dashingTimer >= dashDuration)
            {
                OnAbilityDeactivate();
            }
        } else
        {
            dashCooldownTimer += Time.deltaTime;
        }
    }

    protected override void StartParticles(List<ParticleSystem> particleList)
    {
        base.StartParticles(particleList);
    }

    protected override void StopParticles(List<ParticleSystem> particleList)
    {
        base.StopParticles(particleList);
    }
}
