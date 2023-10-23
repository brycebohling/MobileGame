using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Health))]
public class AIExplode : AIBase
{
    public AIStates.States[] BlockingActionStates;
    [SerializeField] bool explodeOnDeath;
    [SerializeField] float explosionBuildUpTime;
    [SerializeField] float explosionRadius;
    [SerializeField] float explosionTargetingRadius;
    [SerializeField] float explosionDamage;
    [SerializeField] float knockBackForce;
    [SerializeField] AnimationClip explosionBuildUpAnim;
    [SerializeField] Transform explosionVFXTransfrom;
    [SerializeField] LayerMask targetLayer;

    [SerializeField] bool drawGizmos;

    float explosionBuildUpCounter;
    bool startedBuildUpExplosion;


    protected override void Awake()
    {
        base.Awake();
    }

    private void Update()
    {
        if (!IsActionAuth(BlockingActionStates)) return;

        if (explosionBuildUpCounter >= explosionBuildUpTime)
        {
            Explode();

        } else if (IsPlayerInRange(explosionTargetingRadius, targetLayer) && !startedBuildUpExplosion)
        {
            BuildUpExplosion();
        }

        if (startedBuildUpExplosion)
        {
            explosionBuildUpCounter += Time.deltaTime;
        }
    }

    private void BuildUpExplosion()
    {
        _aiPathScript.canMove = false;
        
        _rb.velocity = Vector2.zero;

        Helpers.ChangeAnimationState(_animator, explosionBuildUpAnim.name);

        startedBuildUpExplosion = true;
        _aIStatesScript.State = AIStates.States.Attacking;
    }

    public void Explode()
    {
        Instantiate(explosionVFXTransfrom, transform.position, Quaternion.identity);

        Collider2D hitTarget = Physics2D.OverlapCircle(transform.position, explosionRadius, targetLayer);

        if (hitTarget != null && hitTarget.TryGetComponent(out Health healthScript))
        {
            healthScript.Damage(explosionDamage, knockBackForce, transform.position);
        }

        _healthScript.InstaKill();
    }

    private void OnDrawGizmos()
    {
        if (!drawGizmos) return;

        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}