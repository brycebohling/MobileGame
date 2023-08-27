using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBase : MonoBehaviour
{
    protected Animator _playerAnimator;
    protected Rigidbody2D _playerRb;
    protected SpriteRenderer _playerSpriteSpriteRenderer;

    protected Health _playerHealthScript;
    protected PlayerMovement _playerMovementScript;
    protected PlayerStates _playerStatesScript;
    protected Health _healthScript;

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
        _playerRb = GetComponent<Rigidbody2D>();
        _playerAnimator = gameObject?.GetComponentInChildren<Animator>();
        _playerSpriteSpriteRenderer = gameObject?.GetComponentInChildren<SpriteRenderer>();
        _playerHealthScript = gameObject?.GetComponent<Health>();
        _playerMovementScript = gameObject?.GetComponent<PlayerMovement>();
        _playerStatesScript = gameObject?.GetComponent<PlayerStates>();
        _healthScript = gameObject?.GetComponent<Health>();
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
            if (state == _playerStatesScript.State)
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

    protected virtual void StartSFX()
    {

    }

    protected virtual void StopSFX()
    {
        
    }

    protected virtual void StartAnimation(Animator anim, AnimationClip animClip)
    {
        Helpers.ChangeAnimationState(anim, animClip.name);
    }

    protected virtual void StopAnimation(Animator anim)
    {
        Helpers.ChangeAnimationState(anim, _playerStatesScript.baseAnimationClip.name);
    }
}