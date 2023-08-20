using System.Collections;
using System.Collections.Generic;
using UnityEditor.Callbacks;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CharacterStates))]
public class CharacterDash : CharacterBase
{
    [Header("Blocking States")]
    public CharacterStates.States[] BlockingActionStates;

    [Header("Dash")]
    [SerializeField] float dashSpeedMultiplayer;
    [SerializeField] float dashDuration;
    [SerializeField] float dashCooldown;

    [Header("Particles")]
    [SerializeField] Transform particleSpawn;
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
        if (!IsActionAuth(BlockingActionStates)) return;
        
        OnActionActivate();
    }

    protected override bool IsActionAuth(CharacterStates.States[] blockingActionStates)
    {
        bool isActionNotBlocking = base.IsActionAuth(blockingActionStates);

        if (isActionNotBlocking && dashCooldownTimer >= dashCooldown)
        {
            return true;
        } else
        {
            return false;
        }
    }

    protected override void OnActionActivate()
    {
        _characterStatesScript.State = CharacterStates.States.Dashing;
        velocityBeforeDash = _rb.velocity;
        isDashing = true;

        _rb.AddForce(velocityBeforeDash * dashSpeedMultiplayer, ForceMode2D.Impulse);

        StartParticles(dashParticales, particleSpawn.position);
    }

    protected override void OnActionDeactivate()
    {
        _characterStatesScript.State = CharacterStates.States.Idle;
        isDashing = false;
        dashingTimer = 0;
        dashCooldownTimer = 0;
        _rb.velocity = Vector2.zero;
        
        StopParticles(dashParticales);
    }

    protected override void ProcessCooldowns()
    {
        if (isDashing)
        {
            dashingTimer += Time.deltaTime;
            if (dashingTimer >= dashDuration)
            {
                OnActionDeactivate();
            }
        } else
        {
            dashCooldownTimer += Time.deltaTime;
        }
    }
}
