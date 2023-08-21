using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerFlipper : PlayerBase
{
    [Header("Blocking States")]
    public PlayerStates.States[] BlockingActionStates;

    [Header("Speeds")]
    [SerializeField] float minimumVelocityToFlip; 


    protected override void Start()
    {
        base.Start();    
    }

    void Update()
    {
        if (!IsActionAuth(BlockingActionStates)) return;

        ApplyAction();
    }

    protected override bool IsActionAuth(PlayerStates.States[] blockingActionStates)
    {
        bool isNotBlockingAction = base.IsActionAuth(blockingActionStates);

        if (isNotBlockingAction)
        {
            if (_rb.velocity.x > minimumVelocityToFlip && transform.localScale.x != 1)
            {
                return true;

            } else if (_rb.velocity.x < -minimumVelocityToFlip && transform.localScale.x != -1)
            {
                return true;

            } else
            {
                return false;
            }

        } else
        {
            return false;
        }
    }

    protected override void ApplyAction()
    {
        Vector3 localScale = transform.localScale;
        localScale.x *= -1;    
        transform.localScale = localScale;
    }
}
