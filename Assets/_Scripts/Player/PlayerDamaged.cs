using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Health))]
public class PlayerDamaged : PlayerBase
{
    [SerializeField] float damagedTime;
    [SerializeField] float cameraShakeTime;
    float damageCounter = 0;

    [Header("Animations")]
    [SerializeField] AnimationClip damagedAnim;


    protected override void Awake() 
    {
        base.Start();
    }

    protected override void OnEnable() 
    {
        _healthScript.OnDamaged += Damaged;
    }

    protected override void OnDisable() 
    {
        _healthScript.OnDamaged -= Damaged;
    }

    private void Update() 
    {
        if (!IsActionAuth(null)) return;
        
        damageCounter += Time.deltaTime;
        if (damageCounter >= damagedTime)
        {
            _playerRb.velocity = Vector2.zero;
            _playerStatesScript.State = PlayerStates.States.Idle;
        }
    }

    protected override bool IsActionAuth(PlayerStates.States[] blockingActionStates)
    {
        if (_playerStatesScript.State == PlayerStates.States.Damaged)
        {
            return true;
        } else
        {
            return false;
        }
    }

    private void Damaged(float dmg, float currentHealth, float knockBackForce, Vector2 senderPos)
    {
        KnockBack(knockBackForce, senderPos);
        damageCounter = 0;

        StartAnimation(_playerAnimator, damagedAnim);

        CameraShake.Cam.CameraStartShake(2, 2, cameraShakeTime);

        _playerStatesScript.State = PlayerStates.States.Damaged;
    }

    private void KnockBack(float knockBackForce, Vector2 senderPos)
    {
        Vector2 dir = ((Vector2)transform.position - senderPos).normalized;
        _playerRb.AddForce(dir * knockBackForce, ForceMode2D.Impulse);

        _playerStatesScript.State = PlayerStates.States.Damaged;
    }
}
