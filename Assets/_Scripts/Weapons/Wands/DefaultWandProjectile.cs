using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class DefaultWandProjectile : MonoBehaviour
{
    int enemyLayer = 7;
    float dmg;
    float knockBackForece;
    LayerMask enemyLayerMask;


    public void Initialize(Vector2 direction, float speed, float dmg, float knockBackForece, LayerMask enemyLayerMask)
    {
        this.dmg = dmg;
        this.knockBackForece = knockBackForece;
        this.enemyLayerMask = enemyLayerMask;
        
        GetComponent<Rigidbody2D>().velocity = direction.normalized * speed;

        var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == enemyLayer)
        {
            other.GetComponent<Health>().DamageObject(dmg, knockBackForece, transform.position);
        }

        Destroy(gameObject);
    }
}
