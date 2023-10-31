using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine;

public class AIBase : MonoBehaviour
{
    protected AIStates _aIStatesScript; 
    protected Rigidbody2D _rb;
    protected Animator _animator;
    protected SpriteRenderer _spriteRenderer;
    protected Health _healthScript;
    protected AIPath _aiPathScript;
    protected AISpriteFlipper _aiSpriteFlipper;


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
        _animator = gameObject?.GetComponentInChildren<Animator>();
        _spriteRenderer = gameObject?.GetComponentInChildren<SpriteRenderer>();
        _rb = gameObject?.GetComponent<Rigidbody2D>();
        _healthScript = gameObject?.GetComponent<Health>();
        _aIStatesScript = gameObject?.GetComponent<AIStates>();
        _aiPathScript = gameObject?.GetComponent<AIPath>();
        _aiSpriteFlipper = gameObject?.GetComponent<AISpriteFlipper>();
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

    protected virtual void OnActionActivate()
    {
        _rb.velocity = Vector2.zero;
        _aiPathScript.canMove = true;
    }

    protected virtual void OnActionDeactivate()
    {

    }

    protected virtual void OnActionCancel()
    {

    }

    protected virtual void HandleAction()
    {

    }

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

    protected virtual void InProgressSFX()
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
        Helpers.ChangeAnimationState(anim, _aIStatesScript.baseAnimationClip.name);
    }

    protected virtual bool IsTargetInRange(float attackRadius, LayerMask targetLayer)
    {
        RaycastHit2D hitPlayer = Physics2D.CircleCast(transform.position, attackRadius, Vector2.zero, 0, targetLayer);

        return hitPlayer;
    }
}
