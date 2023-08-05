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
    public CharacterStates.CharacterConditions[] BlockingConditionStates;

    [Header("Speeds")]
    [SerializeField] float walkSpeed;

    [Header("Particales")]
    [SerializeField] List<ParticleSystem> walkParticles;

    

    InputAction movementKeys;
    public Vector2 moveDir;
    Vector2 movementSpeed;
        

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
        SetMovement();
        HandleParticals();
    }

    private void FixedUpdate()
    {
        ProcessAbilityRequest();
    }

    protected override void AbilityActivate()
    {
        StartParticles(walkParticles);
    }

    protected override void AbilityDeactivate()
    {
        StopParticles(walkParticles);
    }
    
    protected override void ProcessAbilityRequest()
    {
        foreach (CharacterStates.MovementStates state in BlockingMovementStates)
        {
            if (state == _characterStatesScript._movementState)
            {
                return;
            }
        }

        if (_characterStatesScript._movementState == CharacterStates.MovementStates.Walking)
        {
            AbilityActivate();
        }

        _characterStatesScript._movementState = CharacterStates.MovementStates.Walking;
        _rb.velocity = movementSpeed;
    }


    protected override void HandleInput()
    {
        moveDir = movementKeys.ReadValue<Vector2>();
    }

    private void SetMovement()
    {
        movementSpeed = moveDir * walkSpeed;
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
