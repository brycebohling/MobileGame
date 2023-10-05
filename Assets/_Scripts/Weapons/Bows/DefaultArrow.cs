using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class DefaultArrow : ProjectileBase
{

    public void Spawn(Vector2 direction, float speed, float dmg, float knockBackForece)
    {
        base.Initialize(direction, speed, dmg, knockBackForece);
    }
}
