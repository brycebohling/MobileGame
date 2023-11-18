using System;
using System.Collections;
using System.Collections.Generic;
using MoreMountains.Feedbacks;
using UnityEditor.Rendering;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(PlayerStates))]
public class PlayerMovement : PlayerBase
{   
    [Header("Blocking States")]
    public PlayerStates.States[] BlockingActionStates;

    [Header("Input")]
    [SerializeField] InputReaderSO inputReaderSO;

    [Header("Speeds")]
    [SerializeField] float walkSpeed;

    [Header("Animations")]
    [SerializeField] AnimationClip walkAnim;

    [Header("MMFeedbacks")]
    [SerializeField] MMF_Player movementFeedBackPlayer;

    

    Vector2 moveDir;
    Vector2 movementSpeed;
    bool isActivated;

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void OnEnable()
    {
        inputReaderSO.MovementEvent += SetMoveDirection;
    }

    protected override void OnDisable()
    {
        inputReaderSO.MovementEvent -= SetMoveDirection;
    }

    void Update()
    {
        if (!IsActionAuth(BlockingActionStates)) 
        {
            if (isActivated)
            {
                OnActionCancel();
            }

            return;
        }

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

    protected override void OnActionCancel()
    {
        isActivated = false;
        movementFeedBackPlayer.ResumeFeedbacks();
    }

    private void SetMoveDirection(Vector2 moveDirection)
    {
        moveDir = moveDirection;
    }

    protected override void HandleInput()
    {
        if (moveDir != Vector2.zero)
        {
            SetMovement();

            if (!isActivated)
            {
                OnActionActivate();
            }

        } else if (_statesScript.State == PlayerStates.States.Walking)
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

        _statesScript.State = PlayerStates.States.Walking;
    }

    protected override void OnActionActivate()
    {
        isActivated = true;

        StartAnimation(_animator, walkAnim);

        movementFeedBackPlayer?.PlayFeedbacks();
    }

    protected override void OnActionDeactivate()
    {
        isActivated = false;

        _rb.velocity = Vector2.zero;
        _statesScript.State = PlayerStates.States.Idle;

        StopAnimation(_animator);
        
        movementFeedBackPlayer.ResumeFeedbacks();
    }
}
