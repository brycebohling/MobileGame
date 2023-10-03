using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class DefaultWandProjectile : MonoBehaviour
{
    float dmg;
    float knockBackForece;


    public void Initialize(Vector2 direction, float speed, float dmg, float knockBackForece)
    {
        this.dmg = dmg;
        this.knockBackForece = knockBackForece;
        
        GetComponent<Rigidbody2D>().velocity = direction.normalized * speed;

        var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        
        if (other.TryGetComponent(out Health healthScript))
        {
            healthScript.DamageObject(dmg, knockBackForece, transform.position);
        }

        Destroy(gameObject);
    }
}
