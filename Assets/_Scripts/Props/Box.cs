using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class Box : MonoBehaviour
{
    [SerializeField] Vector2 center;

    [Header("Health")]
    Health boxHealthScript;
    
    [Header("Particles")]
    [SerializeField] Transform brakeParticles;

    [Header("Animations")]
        Animator animator;
    [System.Serializable]
    public struct BoxBrakingAnims
    {
        public AnimationClip brakingAnim;
        public float triggerHp; 
    }

    [SerializeField] List<BoxBrakingAnims> boxBrakingAnims = new();


    void Awake()
    {
        boxHealthScript = GetComponent<Health>();
    }

    void Start() 
    {
        
        animator = GetComponent<Animator>();

        if (boxBrakingAnims.Count != 0)
        {
            Helpers.ChangeAnimationState(animator, boxBrakingAnims[0].brakingAnim.name);
        }
    }

    void OnEnable() 
    {
        boxHealthScript.OnDamaged += Damage;
        boxHealthScript.OnDeath += Died;
    }

    void OnDisable() 
    {
        boxHealthScript.OnDamaged -= Damage;
        boxHealthScript.OnDeath -= Died;
    }

    void Damage(float dmg, float currentHealth, float knockBackForce, Vector2 senderPos)
    {
        Instantiate(brakeParticles, (Vector2)transform.position + center, Quaternion.identity);

        foreach (BoxBrakingAnims index in boxBrakingAnims)
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
