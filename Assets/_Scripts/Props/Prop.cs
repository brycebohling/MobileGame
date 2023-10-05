using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class Prop : MonoBehaviour
{
    [Header("Size")]
    [SerializeField] Vector2 center;

    [Header("Health")]
    [SerializeField] bool isBrakeable;
    Health healthScript;
    
    [Header("Particles")]
    [SerializeField] Transform brakeParticles;

    [Header("Animations")]
    Animator animator;
    [System.Serializable] public struct BrakingAnims
    {
        public AnimationClip brakingAnim;
        public float triggerHp; 
    }

    [SerializeField] List<BrakingAnims> brakingAnimsList = new();


    void Awake()
    {
        if (!isBrakeable) return;

        healthScript = GetComponent<Health>();
    }

    void Start() 
    {
        
        animator = GetComponent<Animator>();

        if (!isBrakeable) return;

        if (brakingAnimsList.Count != 0)
        {
            Helpers.ChangeAnimationState(animator, brakingAnimsList[0].brakingAnim.name);
        }
    }

    void OnEnable() 
    {
        if (!isBrakeable) return;

        healthScript.OnDamaged += Damage;
        healthScript.OnDeath += Died;
    }

    void OnDisable() 
    {
        if (!isBrakeable) return;

        healthScript.OnDamaged -= Damage;
        healthScript.OnDeath -= Died;
    }

    void Damage(float dmg, float currentHealth, float knockBackForce, Vector2 senderPos)
    {
        Instantiate(brakeParticles, (Vector2)transform.position + center, Quaternion.identity);

        foreach (BrakingAnims index in brakingAnimsList)
        {
            if (index.triggerHp == currentHealth)
            {
                Helpers.ChangeAnimationState(animator, index.brakingAnim.name);
                break;
            }
        }
    }

    void Died()
    {
        Instantiate(brakeParticles, (Vector2)transform.position + center, Quaternion.identity);

        Destroy(gameObject);
    }
}
