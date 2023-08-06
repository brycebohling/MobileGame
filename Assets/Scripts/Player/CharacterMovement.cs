using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class CharacterMovement : CharacterBase
{   
    [Header("Blocking States")]
    public CharacterStates.MovementStates[] BlockingMovementStates;
    public CharacterStates.CharacterConditions[] BlockingAbilityStates;

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

        } else if (_characterStatesScript.movementState == CharacterStates.MovementStates.Walking)
        {
            _rb.velocity = Vector2.zero;
            canUseAbility = false;
            AbilityDeactivate();

            _characterStatesScript.movementState = CharacterStates.MovementStates.Idle;
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
            if (state == _characterStatesScript.movementState)
            {
                if (firstTimeBlocked) 
                {
                    AbilityDeactivate();
                    firstTimeBlocked = false;
                }

                canUseAbility = false;
                return;
            }
        }

        if (isAbilityDeactivated)
        {
            AbilityActivate();
        }

        _characterStatesScript.movementState = CharacterStates.MovementStates.Walking;
        canUseAbility = true;
    }

    protected override void ApplyAbility()
    {
        _rb.velocity = movementSpeed;
    }

    protected override void AbilityActivate()
    {
        StartParticles(walkParticles);
        isAbilityDeactivated = false;
        firstTimeBlocked = true;
    }

    protected override void AbilityDeactivate()
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
