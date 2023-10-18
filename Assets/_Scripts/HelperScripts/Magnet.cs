using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Magnet : MonoBehaviour
{
    public UnityEvent OnCollisionWithTarget;

    [Header("Magnet")]
    [SerializeField] LayerMask targetLayerMask;
    [SerializeField] float magnetRange;
    [SerializeField, Min(0)] float magnetStrength;
    [SerializeField] AnimationCurve magnetCurve;

    [Header("Collisions")]
    [SerializeField] float collisionRadius;


    private void FixedUpdate()
    {
        Collider2D hitTarget = Physics2D.OverlapCircle(transform.position, magnetRange, targetLayerMask);

        if (hitTarget != null)
        {
            Vector2 hitTargetPosition = hitTarget.gameObject.transform.position;

            float curveValue = magnetCurve.Evaluate((magnetRange - Vector2.Distance(transform.position, hitTargetPosition)) / magnetRange);

            transform.position = Vector2.Lerp(transform.position, hitTargetPosition, curveValue * magnetStrength);

            if (Physics2D.OverlapCircle(transform.position, collisionRadius, targetLayerMask))
            {
                OnCollisionWithTarget?.Invoke();
            }
        }
    }
}
