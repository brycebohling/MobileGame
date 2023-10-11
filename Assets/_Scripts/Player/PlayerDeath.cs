using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Health))]
public class PlayerDeath : PlayerBase
{
    public UnityEvent OnDeath;

    [Header("Weapon")]
    [SerializeField] Transform weaponHolder;

    [Header("Particles")]
    [SerializeField] List<ParticleSystem> deathParticles = new();


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
        _spriteRenderer.enabled = false;

        weaponHolder?.gameObject.SetActive(false);

        InstantiateParticales(deathParticles, transform.position);

        OnDeath?.Invoke();
    }
}
