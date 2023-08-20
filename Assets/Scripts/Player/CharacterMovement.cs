using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CharacterStates))]
public class CharacterMovement : CharacterBase
{   
    [Header("Blocking States")]
    public CharacterStates.States[] BlockingActionStates;

    [Header("Speeds")]
    [SerializeField] float walkSpeed;

    [Header("Particales")]
    [SerializeField] Transform particleSpawn;
    [SerializeField] List<ParticleSystem> walkParticles;

    
    InputAction movementKeys;
    public Vector2 moveDir;
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

        } else if (_characterStatesScript.State == CharacterStates.States.Walking)
        {
            _rb.velocity = Vector2.zero;
            _characterStatesScript.State = CharacterStates.States.Idle;

            OnActionDeactivate();
        }
    }

    private void SetMovement()
    {
        movementSpeed = moveDir * walkSpeed;
    }

    protected override bool IsActionAuth(CharacterStates.States[] blockingActionStates)
    {
        return base.IsActionAuth(blockingActionStates);
    }

    protected override void ApplyAction()
    {
        _rb.velocity = movementSpeed;

        _characterStatesScript.State = CharacterStates.States.Walking;
    }

    protected override void OnActionActivate()
    {
        isActivated = true;

        StartParticles(walkParticles, particleSpawn.position);
    }

    protected override void OnActionDeactivate()
    {
        isActivated = false;

        StopParticles(walkParticles);
    }
}
