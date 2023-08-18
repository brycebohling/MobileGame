using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AICollisions : AIBase
{
    [SerializeField] float collisionDmg;
    [SerializeField] float knockBackForce;


    private void OnTriggerEnter2D(Collider2D other) 
    {
        if (other.gameObject.CompareTag("Player"))      
        {
            other.gameObject.GetComponent<Health>().DamageObject(collisionDmg, knockBackForce, transform.position);
        }
    }

    private void OnTriggerStay2D(Collider2D other) 
    {
        if (other.gameObject.CompareTag("Player"))      
        {
            other.gameObject.GetComponent<Health>().DamageObject(collisionDmg, knockBackForce, transform.position);
        }
    }
}
