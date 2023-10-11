using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIDeath : AIBase
{
    [Header("Particales")]
    [SerializeField] List<ParticleSystem> deathParticles;
    [SerializeField] float particleTime;


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
        _aIStatesScript.State = AIStates.States.Dead;

        InstantiateParticales(deathParticles, transform.position);

        Destroy(gameObject);
    }
}
