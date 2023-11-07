using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBase : MonoBehaviour
{
    protected Animator _animator;
    protected Rigidbody2D _rb;
    protected SpriteRenderer _spriteRenderer;

    protected Health _healthScript;
    protected PlayerMovement _movementScript;
    protected PlayerStates _statesScript;

    float _spriteCenterOffset = 0.6f;


    protected virtual void Awake()
    {   
        Initialization();
    }

    protected virtual void Start() 
    {
        Initialization();
    }

    private void Initialization()
    {
        _rb = GetComponent<Rigidbody2D>();
        _animator = gameObject?.GetComponentInChildren<Animator>();
        _spriteRenderer = gameObject?.GetComponentInChildren<SpriteRenderer>();
        _healthScript = gameObject?.GetComponent<Health>();
        _movementScript = gameObject?.GetComponent<PlayerMovement>();
        _statesScript = gameObject?.GetComponent<PlayerStates>();
    }

    protected virtual void OnEnable()
    {
    
    }

    protected virtual void OnDisable()
    {

    }

    // HandleInputs
    protected virtual void HandleInput()
    {
       
    }

    protected virtual bool IsActionAuth(PlayerStates.States[] blockingActionStates)
    {
        foreach (PlayerStates.States state in blockingActionStates)
        {
            if (state == _statesScript.State)
            {
                return false;
            }
        }
        return true;
    }

    protected virtual void ApplyAction()
    {

    }

    // Runs onces when the ability is activated
    protected virtual void OnActionActivate()
    {

    }

    // Runs onces when the ability is dectivated
    protected virtual void OnActionDeactivate()
    {

    }

    // Handle ability cooldowns
    protected virtual void ProcessCooldowns()
    {

    }

    protected virtual void HandleParticles()
    {
        
    }

    protected virtual void StartParticles(List<ParticleSystem> particleList, Vector3 pos)
    {
        foreach (ParticleSystem particleSystem in particleList)
        {
            if (!particleSystem.isEmitting)
            {
                particleSystem.transform.position = pos;

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

    protected virtual void InstantiateParticales(List<ParticleSystem> particleList, Vector3 pos)
    {
        foreach (ParticleSystem particleSystem in particleList)
        {
            Instantiate(particleSystem, pos, Quaternion.identity);
        }
    }

    protected virtual void StartSFX()
    {

    }

    protected virtual void StopSFX()
    {
        
    }

    protected virtual void StartAnimation(Animator anim, AnimationClip animClip)
    {
        Helpers.ChangeAnimationState(anim, animClip.name, 1);
    }

    protected virtual void StopAnimation(Animator anim)
    {
        Helpers.ChangeAnimationState(anim, _statesScript.baseAnimationClip.name, 1);
    }

    protected Vector2 GetPlayerCenter()
    {
        Vector2 playerCenter = new(_spriteRenderer.transform.position.x, 
            _spriteRenderer.transform.position.y + _spriteCenterOffset);
            
        return playerCenter;
    }
}