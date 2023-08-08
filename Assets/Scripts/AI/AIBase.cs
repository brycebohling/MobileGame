using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIBase : MonoBehaviour
{
    protected AIStates _aIStatesScript; 
    protected Rigidbody2D _rb;
    protected Animator _animator;
    protected SpriteRenderer _spriteRenderer;
    protected Health _healthScript;

    

    protected virtual void Awake()
    {
        
    }

    protected virtual void Start() 
    {
        Initialization();
    }

    private void Initialization()
    {
        _rb = gameObject?.GetComponent<Rigidbody2D>();
        _animator = gameObject?.GetComponent<Animator>();
        _spriteRenderer = gameObject?.GetComponentInChildren<SpriteRenderer>();
        _healthScript = gameObject?.GetComponent<Health>();
        _aIStatesScript = gameObject?.GetComponent<AIStates>();
    }

    protected virtual void OnEnable()
    {
    
    }

    protected virtual void OnDisable()
    {

    }

    protected virtual bool IsActionAuth(AIStates.States[] blockingActionStates)
    {
        foreach (AIStates.States state in blockingActionStates)
        {
            if (state == _aIStatesScript.State)
            {
                return false;
            }
        }

        return true;
    }

    protected virtual void RestAction()
    {

    }

    protected virtual void HandleAction()
    {

    }

    protected virtual void ActionActivate()
    {

    }

    protected virtual void ActionDeactivate()
    {

    }

    protected virtual void ProcessCooldowns()
    {

    }

    protected virtual void HandleParticles()
    {
        
    }

    protected virtual void StartParticles(List<ParticleSystem> particleList)
    {
        foreach (ParticleSystem particleSystem in particleList)
        {
            if (!particleSystem.isEmitting)
            {
                particleSystem.Play();
                ParticleSystem.EmissionModule em = particleSystem.GetComponent<ParticleSystem>().emission;
                em.enabled = true;
            }
        }
    }

    protected virtual void StopParticles(List<ParticleSystem> particleList)
    {
        foreach (ParticleSystem particleSystem in particleList)
        {
            if (particleSystem.isEmitting)
            {
                particleSystem.Stop();
                ParticleSystem.EmissionModule em = particleSystem.GetComponent<ParticleSystem>().emission;
                em.enabled = false;   
            }
        }
    }

    protected virtual void StartSFX()
    {

    }

    protected virtual void InProgressSFX()
    {

    }

    protected virtual void StopSFX()
    {
        
    }

    protected virtual void StartAnimation()
    {

    }

    protected virtual void InProgressAnimation()
    {
        
    }

    protected virtual void StopAnimation()
    {
        
    }

    protected virtual void Death()
    {
        
    }

    protected virtual void Respawn()
    {
		
    }
}
