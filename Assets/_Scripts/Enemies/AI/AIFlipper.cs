using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class AIFlipper : AIBase
{
    public AIStates.States[] BlockingActionStates;
    [SerializeField] Transform visualTransform;

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

        }
        else if (_aiPathScript.velocity.x < 0 && isFacingRight)
        {
            Flip();
        }
    }

    private void Flip()
    {
        if (visualTransform == null) return;
        Vector2 newScale = visualTransform.localScale;
        newScale.x *= -1;
        visualTransform.localScale = newScale;

        isFacingRight = !isFacingRight;
    }

    public void FlipTowardsTarget(float targetXPos)
    {
        if (targetXPos < transform.position.x && isFacingRight)
        {
            Flip();

        }
        else if (targetXPos > transform.position.x && !isFacingRight)
        {
            Flip();
        }
    }
}