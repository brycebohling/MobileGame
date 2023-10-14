using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileBase : MonoBehaviour
{
    protected float _dmg;
    protected float _knockBackForece;


    protected virtual void Initialize(Vector2 direction, float speed, float dmg, float knockBackForece)
    {
        _dmg = dmg;
        _knockBackForece = knockBackForece;
        
        GetComponent<Rigidbody2D>().velocity = direction.normalized * speed;

        var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {        
        if (other.TryGetComponent(out Health healthScript))
        {
            healthScript.DamageObject(_dmg, _knockBackForece, transform.position);
        }

        Destroy(gameObject);
    }
}
