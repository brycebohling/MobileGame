using System.Collections;
using System.Collections.Generic;
using UnityEditor.Callbacks;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(PlayerStates))]
public class PlayerDash : PlayerBase
{
    [Header("Blocking States")]
    public PlayerStates.States[] BlockingActionStates;

    [Header("Dash")]
    [SerializeField] float dashSpeedMultiplayer;
    [SerializeField] float dashDuration;
    [SerializeField] float dashCooldown;

    [Header("Animations")]
    [SerializeField] AnimationClip dashAnim;

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

    protected override bool IsActionAuth(PlayerStates.States[] blockingActionStates)
    {
        bool isNotBlockingAction = base.IsActionAuth(blockingActionStates);

        if (isNotBlockingAction && dashCooldownTimer >= dashCooldown)
        {
            return true;
        } else
        {
            return false;
        }
    }

    protected override void OnActionActivate()
    {
        _playerStatesScript.State = PlayerStates.States.Dashing;
        velocityBeforeDash = _playerRb.velocity;
        isDashing = true;

        _playerRb.AddForce(velocityBeforeDash * dashSpeedMultiplayer, ForceMode2D.Impulse);

        StartAnimation(_playerAnimator, dashAnim);

        StartParticles(dashParticales, particleSpawn.position);
    }

    protected override void OnActionDeactivate()
    {
        _playerStatesScript.State = PlayerStates.States.Idle;
        isDashing = false;
        dashingTimer = 0;
        dashCooldownTimer = 0;
        _playerRb.velocity = Vector2.zero;
        
        StopAnimation(_playerAnimator);

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