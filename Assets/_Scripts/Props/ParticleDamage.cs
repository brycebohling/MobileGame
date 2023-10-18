using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleDamage : MonoBehaviour
{
    Health playerHealthScript;
    BoxCollider2D playerCollider;
    ParticleSystem ps;

    [SerializeField] float particleDamage;
    [SerializeField] float knockBackForce;


    private void Awake()
    {
        ps = GetComponent<ParticleSystem>();
    }

    private void Start()
    {
        playerHealthScript = GameManager.Gm.playerTransfrom.GetComponent<Health>();
        playerCollider = playerHealthScript.GetComponent<BoxCollider2D>();

        ps.trigger.AddCollider(playerCollider);
    }

    void OnParticleTrigger() 
    {
        playerHealthScript.Damage(particleDamage, knockBackForce, transform.position);   
    }
}
