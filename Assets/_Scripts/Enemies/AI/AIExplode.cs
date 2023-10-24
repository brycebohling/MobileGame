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

    [SerializeField] Renderer parentRenderer;

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
            Kill();
            explosionBuildUpCounter = 0;

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

    private void Kill()
    {
        _healthScript.InstaKill();
    }

    // Called by UnityEvent
    public void Explode()
    {
        Transform explosionTransfrom = Instantiate(explosionVFXTransfrom, transform.position, Quaternion.identity);

        Renderer explosionRenderer = explosionTransfrom.GetComponent<Renderer>();

        Debug.Log(parentRenderer.material.GetFloat("_HsvShift"));
        
        explosionRenderer.material.SetFloat("_HsvShift", parentRenderer.material.GetInt("_HsvShift"));
        explosionRenderer.material.SetFloat("_Contrast", parentRenderer.material.GetInt("_Contrast"));
        explosionRenderer.material.SetFloat("_Brightness", parentRenderer.material.GetInt("_Brightness"));

        Collider2D hitTarget = Physics2D.OverlapCircle(transform.position, explosionRadius, targetLayer);

        if (hitTarget != null && hitTarget.TryGetComponent(out Health healthScript))
        {
            healthScript.Damage(explosionDamage, knockBackForce, transform.position);
        }        
    }

    private void OnDrawGizmos()
    {
        if (!drawGizmos) return;

        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}