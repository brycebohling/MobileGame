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
        _animator = gameObject?.GetComponentInChildren<Animator>();
        _spriteRenderer = gameObject?.GetComponentInChildren<SpriteRenderer>();
        _healthScript = gameObject?.GetComponent<Health>();
        _aIStatesScript = gameObject?.GetComponent<AIStates>();
        _aiPathScript = gameObject?.GetComponent<AIPath>();
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

    }

    protected virtual void OnActionDeactivate()
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

    protected virtual bool IsPlayerInRange(float attackRadius, LayerMask playerLayer)
    {
        RaycastHit2D[] hitPlayers = Physics2D.CircleCastAll(transform.position, attackRadius, Vector2.zero, 0, playerLayer);

        if (hitPlayers.Length != 0)
        {
            return true;
        } else
        {
            return false;
        }
    }

    protected virtual Vector2 FindClosesetPlayerInRange(float attackRadius, LayerMask playerLayer)
    {
        RaycastHit2D[] hitPlayers = Physics2D.CircleCastAll(transform.position, attackRadius, Vector2.zero, 0, playerLayer);

        if (hitPlayers.Length != 0)
        {
            Vector2 closestPlayerDistance = hitPlayers[0].transform.position - transform.position;
            Vector2 closestPlayerPos = hitPlayers[0].transform.position;

            foreach (RaycastHit2D hitPlayer in hitPlayers)
            {
                if (((Vector2)hitPlayer.transform.position - (Vector2)transform.position).sqrMagnitude < closestPlayerDistance.sqrMagnitude)
                {
                    closestPlayerDistance = hitPlayer.transform.position - transform.position;
                    closestPlayerPos = hitPlayer.transform.position;
                }
            }

            return closestPlayerPos;
        }

        Debug.Log("Should never run this.");
        return Vector2.zero;
    }
}
