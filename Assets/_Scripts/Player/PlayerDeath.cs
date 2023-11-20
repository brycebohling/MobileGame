using System.Collections;
using System.Collections.Generic;
using MoreMountains.Feedbacks;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Health))]
public class PlayerDeath : PlayerBase
{
    public UnityEvent OnDeath;


    [Header("MMFeedacks")]
    [SerializeField] MMF_Player deathFeedbacks;
    


    protected override void Awake()
    {
        base.Awake();
    }

    protected override void OnEnable() 
    {
        _healthScript.OnDeath += Death;
    }

    protected override void OnDisable() 
    {
        _healthScript.OnDeath -= Death;
    }

    private void Death()
    {
        _statesScript.State = PlayerStates.States.Dead;
        
        _rb.velocity = Vector2.zero;

        deathFeedbacks?.PlayFeedbacks();

        OnDeath?.Invoke();
    }
}
