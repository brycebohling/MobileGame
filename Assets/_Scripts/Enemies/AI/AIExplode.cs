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

    [SerializeField] SpriteRenderer parentSpriteRenderer;
    [SerializeField] Material baseMat;

    [SerializeField] bool drawGizmos;

    float explosionBuildUpCounter;
    bool startedBuildUpExplosion;


    protected override void Awake()
    {
        base.Awake();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Explode();
        }

        if (!IsActionAuth(BlockingActionStates)) return;

        if (explosionBuildUpCounter >= explosionBuildUpTime)
        {
            Kill();
            explosionBuildUpCounter = 0;

        } else if (IsTargetInRange(explosionTargetingRadius, targetLayer) && !startedBuildUpExplosion)
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

        Helpers.ChangeAnimationState(_animator, explosionBuildUpAnim.name, 1);
        StartCoroutine(PlayExplosionColors());

        startedBuildUpExplosion = true;
        _aIStatesScript.State = AIStates.States.Attacking;
    }

    private void Kill()
    {
        _healthScript.InstaKill();
    }

    private IEnumerator PlayExplosionColors()
    {
        Material parentMat = parentSpriteRenderer.material;

        float counter = 0;

        while (counter < explosionBuildUpAnim.length)
        {
            parentMat.SetFloat("_HsvShift", Mathf.Lerp(parentMat.GetFloat("_HsvShift"), 110, counter / explosionBuildUpAnim.length));
            parentMat.SetFloat("_Contrast", Mathf.Lerp(parentMat.GetFloat("_Contrast"), 3, counter / explosionBuildUpAnim.length));
            parentMat.SetFloat("_Brightness", Mathf.Lerp(parentMat.GetFloat("_Brightness"), -1, counter / explosionBuildUpAnim.length));

            counter += Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }
    }

    // Called by UnityEvent
    public void Explode()
    {
        Transform explosionTransfrom = Instantiate(explosionVFXTransfrom, transform.position, Quaternion.identity);

        Material parentMat = parentSpriteRenderer.material;

        Material explosionMat = explosionTransfrom.GetComponent<SpriteRenderer>().material;
        
        explosionMat.SetFloat("_HsvShift", parentMat.GetInt("_HsvShift"));
        explosionMat.SetFloat("_Contrast", parentMat.GetInt("_Contrast"));
        explosionMat.SetFloat("_Brightness", parentMat.GetInt("_Brightness"));

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