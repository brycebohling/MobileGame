using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Prop : MonoBehaviour
{
    [Header("Events")]
    public UnityEvent OnDeath;

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


    private void Awake()
    {
        if (!isBrakeable) return;

        healthScript = GetComponent<Health>();
    }

    private void Start() 
    {
        animator = GetComponent<Animator>();

        if (!isBrakeable) return;

        if (brakingAnimsList.Count != 0)
        {
            Helpers.ChangeAnimationState(animator, brakingAnimsList[0].brakingAnim.name, 1);
        }
    }

    private void OnEnable() 
    {
        if (!isBrakeable) return;

        healthScript.OnDamaged += Damage;
        healthScript.OnDeath += Died;
    }

    private void OnDisable() 
    {
        if (!isBrakeable) return;

        healthScript.OnDamaged -= Damage;
        healthScript.OnDeath -= Died;
    }

    private void Damage(float dmg, float currentHealth, float knockBackForce, Vector2 senderPos)
    {
        SpawnParticles();

        foreach (BrakingAnims index in brakingAnimsList)
        {
            if (index.triggerHp == currentHealth)
            {
                Helpers.ChangeAnimationState(animator, index.brakingAnim.name, 1);
                break;
            }
        }
    }

    private void Died()
    {
        SpawnParticles();

        OnDeath?.Invoke();
        
        Destroy(gameObject);
    }

    private void SpawnParticles()
    {
        if (brakeParticles == null) return;

        Instantiate(brakeParticles, transform.position, Quaternion.identity);
    }
}