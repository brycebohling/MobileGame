using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(PlayerStates))]
public class PlayerMovement : PlayerBase
{   
    [Header("Blocking States")]
    public PlayerStates.States[] BlockingActionStates;

    [Header("Speeds")]
    [SerializeField] float walkSpeed;

    [Header("Animations")]
    [SerializeField] AnimationClip walkAnim;

    [Header("Particales")]
    [SerializeField] Transform particleSpawn;
    [SerializeField] List<ParticleSystem> walkParticles;

    
    InputAction movementKeys;
    Vector2 moveDir;
    Vector2 movementSpeed;

    bool isActivated;

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Start()
    {
        base.Start();
    }

    protected override void OnEnable()
    {
        movementKeys = _inputManager.Player.Movement;
        movementKeys.Enable();
    }

    protected override void OnDisable()
    {
        movementKeys.Disable();
    }

    void Update()
    {
        if (!IsActionAuth(BlockingActionStates)) return;

        HandleInput();
    }

    private void FixedUpdate()
    {
        if (!IsActionAuth(BlockingActionStates)) return;
        
        if (moveDir != Vector2.zero)
        {
            ApplyAction();
        }
    }

    protected override void HandleInput()
    {
        moveDir = movementKeys.ReadValue<Vector2>();
        
        if (moveDir != Vector2.zero)
        {
            SetMovement();

            if (!isActivated)
            {
                OnActionActivate();
            }

        } else if (_playerStatesScript.State == PlayerStates.States.Walking)
        {
            OnActionDeactivate();
        }
    }

    private void SetMovement()
    {
        movementSpeed = moveDir * walkSpeed;
    }

    protected override bool IsActionAuth(PlayerStates.States[] blockingActionStates)
    {
        return base.IsActionAuth(blockingActionStates);
    }

    protected override void ApplyAction()
    {
        _rb.velocity = movementSpeed;

        _playerStatesScript.State = PlayerStates.States.Walking;
    }

    protected override void OnActionActivate()
    {
        isActivated = true;

        StartAnimation(_animator, walkAnim);

        StartParticles(walkParticles, particleSpawn.position);
    }

    protected override void OnActionDeactivate()
    {
        isActivated = false;
        _rb.velocity = Vector2.zero;
        _playerStatesScript.State = PlayerStates.States.Idle;

        StopAnimation(_animator);

        StopParticles(walkParticles);
    }
}
