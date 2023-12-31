using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AICollisions : AIBase
{
    public AIStates.States[] BlockingActionStates;
    [SerializeField] float collisionDmg;
    [SerializeField] float knockBackForce;


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!IsActionAuth(BlockingActionStates)) return;

        if (other.gameObject.CompareTag("Player"))      
        {
            other.gameObject.GetComponent<Health>().Damage(collisionDmg, knockBackForce, transform.position);
        }
    }

    private void OnTriggerStay2D(Collider2D other) 
    {
        if (!IsActionAuth(BlockingActionStates)) return;
        
        if (other.gameObject.CompareTag("Player"))      
        {
            other.gameObject.GetComponent<Health>().Damage(collisionDmg, knockBackForce, transform.position);
        }
    }
}
