using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AICollisions : AIBase
{
    [SerializeField] float collisionDmg;


    private void OnCollisionEnter2D(Collision2D other) 
    {
        if (other.gameObject.CompareTag("Player"))      
        {
            other.gameObject.GetComponent<Health>().DamageObject(collisionDmg, transform.position);
        }
    }
}
