using System.Collections;
using System.Collections.Generic;
using MoreMountains.Tools;
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

    MMHealthBar healthBar;


    protected override void Awake()
    {
        base.Awake();

        healthBar = gameObject?.GetComponent<MMHealthBar>();
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

        if (healthBar != null)
        {
            healthBar.UpdateBar(0, 0, 1, false);
        }
    
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
