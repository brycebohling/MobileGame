using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterBase : MonoBehaviour
{
    protected Animator _animator;
    protected Rigidbody2D _rb;

    protected SpriteRenderer _characterSprite;
    protected Health _characterHealthScript;
    protected CharacterMovement _characterMovementScript;
    protected CharacterStates _characterStatesScript;

    protected InputManager _inputManager;


    
    protected virtual void Awake()
    {
        _inputManager = new InputManager();
    }

    protected virtual void Start() 
    {
        Initialization();
    }

    private void Initialization()
    {
        _rb = GetComponent<Rigidbody2D>();
        _animator = gameObject?.GetComponent<Animator>();
        _characterSprite = gameObject?.GetComponentInChildren<SpriteRenderer>();
        _characterHealthScript = gameObject?.GetComponent<Health>();
        _characterMovementScript = gameObject?.GetComponent<CharacterMovement>();
        _characterStatesScript = gameObject?.GetComponent<CharacterStates>();
    }

    protected virtual void OnEnable()
    {
    
    }

    protected virtual void OnDisable()
    {

    }

    protected virtual void ProcessAbility()
    {

    }

    protected virtual void ResetAbility()
    {

    }

    protected virtual void HandleInput()
    {
       
    }

    protected virtual void HandleParticals()
    {

    }


    protected virtual void StartParticles(List<ParticleSystem> particleList)
    {
        foreach (ParticleSystem particleSystem in particleList)
        {
            if (!particleSystem.isPlaying)
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
            if (particleSystem.isPlaying)
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