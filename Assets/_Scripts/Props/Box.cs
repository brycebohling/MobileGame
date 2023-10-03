using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : MonoBehaviour, IPropDamageable
{
    [SerializeField] Vector2 center;

    [Header("Health")]
    [SerializeField] int hp;
    
    [Header("Particles")]
    [SerializeField] Transform brakeParticles;


    void IPropDamageable.Damage()
    {
        Instantiate(brakeParticles, (Vector2)transform.position + center, Quaternion.identity);

        hp--;

        if (hp <= 0)
        {
            Destroy(gameObject);
        }
    }
}
