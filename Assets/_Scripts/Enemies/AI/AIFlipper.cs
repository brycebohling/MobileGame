using System.Collections.Generic;
using MoreMountains.Tools;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class AIFlipper : AIBase
{
    public AIStates.States[] BlockingActionStates;
    [Tooltip("Automactically flips attached GameObject")]
    [SerializeField] List<Transform> doNotFlipTransfroms;

    bool isFacingRight = true;


    protected override void Awake()
    {
        base.Awake();
    }

    private void Update()
    {
        if (!IsActionAuth(BlockingActionStates)) return;

        if (_aiPathScript.velocity.x > 0 && !isFacingRight)
        {
            Flip();

        } else if (_aiPathScript.velocity.x < 0 && isFacingRight)
        {
            Flip();
        }
    }

    private void Flip()
    {
        Vector2 newScale = transform.localScale;
        newScale.x *= -1;
        transform.localScale = newScale;
        
        isFacingRight = !isFacingRight;

        foreach (Transform trans in doNotFlipTransfroms)
        {
            Vector2 revertFlipScale = trans.localScale;
            revertFlipScale.x *= -1;
            trans.localScale = revertFlipScale;           
        }        
    } 

    public void FlipTowardsTarget(float targetXPos)
    {
        if (targetXPos < transform.position.x && isFacingRight)
        {   
            Flip();

        } else if (targetXPos > transform.position.x && !isFacingRight)
        {
            Flip();
        }
    }
}