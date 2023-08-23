using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerFlipper : PlayerBase
{
    [Header("Blocking States")]
    public PlayerStates.States[] BlockingActionStates;

    [Header("FlipTowardsWeapon")]
    [SerializeField] bool isFlippingTowardsWeapon; // Requires PlayerWeaponMouseFollower Script

    [Header("Speeds")]
    [SerializeField] float minimumVelocityToFlip; 
    
    PlayerWeaponMouseFollower playerWeaponMouseFollowerScript;

    protected override void Start()
    {
        base.Start();

        if (isFlippingTowardsWeapon)
        {
            playerWeaponMouseFollowerScript = transform.root.GetComponent<PlayerWeaponMouseFollower>();
        }
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
            if (isFlippingTowardsWeapon)
            {
                return CanFlipTowardsWeapon();

            } else
            {
                return CanFlipTowardsVelocity();
            }

        } else
        {
            return false;
        }
    }

    private bool CanFlipTowardsWeapon()
    {
        if (playerWeaponMouseFollowerScript.isWeaponOnTheRight && _playerSpriteSpriteRenderer.transform.localScale.x != 1)
        {
            return true;

        } else if (!playerWeaponMouseFollowerScript.isWeaponOnTheRight && _playerSpriteSpriteRenderer.transform.localScale.x != -1)
        {
            return true;
            
        } else
        {
            return false;
        }
    }

    private bool CanFlipTowardsVelocity()
    {
        if (_playerRb.velocity.x > minimumVelocityToFlip && _playerSpriteSpriteRenderer.transform.localScale.x != 1)
        {
            return true;

        } else if (_playerRb.velocity.x < -minimumVelocityToFlip && _playerSpriteSpriteRenderer.transform.localScale.x != -1)
        {
            return true;

        } else
        {
            return false;
        }
    }

    protected override void ApplyAction()
    {
        Vector3 spriteLocalScale = _playerSpriteSpriteRenderer.transform.localScale;
        spriteLocalScale.x *= -1;    
        _playerSpriteSpriteRenderer.transform.localScale = spriteLocalScale;
    }
}
