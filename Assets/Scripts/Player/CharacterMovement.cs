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
    public CharacterStates.MovementStates[] BlockingMovementStates;

    [Header("Speeds")]
    [SerializeField] float walkSpeed;

    [Header("Particales")]
    [SerializeField] List<ParticleSystem> walkParticles;

    
    InputAction movementKeys;
    public Vector2 moveDir;
    Vector2 movementSpeed;

    bool firstTimeBlocked = true;
    bool canUseAbility;
    bool isAbilityDeactivated = true;
        

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
        HandleInput();
    }

    private void FixedUpdate()
    {
        if (canUseAbility)
        {
            ApplyAbility();
        }
    }

    protected override void HandleInput()
    {
        moveDir = movementKeys.ReadValue<Vector2>();
        
        if (moveDir != Vector2.zero)
        {
            SetMovement();
            ProcessAbilityRequest();

        } else if (_characterStatesScript.MovementState == CharacterStates.MovementStates.Walking)
        {
            _rb.velocity = Vector2.zero;
            canUseAbility = false;
            OnAbilityDeactivate();

            _characterStatesScript.MovementState = CharacterStates.MovementStates.Idle;
        }
    }

    private void SetMovement()
    {
        movementSpeed = moveDir * walkSpeed;
    }

    protected override void ProcessAbilityRequest()
    {
        foreach (CharacterStates.MovementStates state in BlockingMovementStates)
        {
            if (state == _characterStatesScript.MovementState)
            {
                if (firstTimeBlocked) 
                {
                    OnAbilityDeactivate();
                    firstTimeBlocked = false;
                }

                canUseAbility = false;
                return;
            }
        }

        if (isAbilityDeactivated)
        {
            OnAbilityActivate();
        }

        _characterStatesScript.MovementState = CharacterStates.MovementStates.Walking;
        canUseAbility = true;
    }

    protected override void ApplyAbility()
    {
        _rb.velocity = movementSpeed;
    }

    protected override void OnAbilityActivate()
    {
        StartParticles(walkParticles);
        isAbilityDeactivated = false;
        firstTimeBlocked = true;
    }

    protected override void OnAbilityDeactivate()
    {
        StopParticles(walkParticles);

        isAbilityDeactivated = true;
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
