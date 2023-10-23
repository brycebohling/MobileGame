using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class AISpriteFlipper : AIBase
{
    bool isFacingRight = true;


    protected override void Awake()
    {
        base.Awake();
    }

    private void Update()
    {
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
        Vector2 tempScale = transform.localScale;
        tempScale.x *= -1;
        transform.localScale = tempScale;

        isFacingRight = !isFacingRight;
    }
}