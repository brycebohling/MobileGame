using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Events;

public class AIDeath : AIBase
{
    public UnityEvent OnDeath;

    [Header("Objects")]
    [SerializeField] Transform objectOnDeath;

    [Header("Particales")]
    [SerializeField] bool spawnParticlesOnDeath;
    [SerializeField] List<ParticleSystem> deathParticles;


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

        if (spawnParticlesOnDeath)
        {
            InstantiateParticales(deathParticles, transform.position);
        }

        if (objectOnDeath != null)
        {
            Instantiate(objectOnDeath, transform.position, Quaternion.identity);
        }

        OnDeath?.Invoke();

        Destroy(gameObject);
    }
}
