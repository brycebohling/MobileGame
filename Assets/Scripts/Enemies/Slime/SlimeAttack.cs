using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(AIStates))]
public class SlimeAttack : MonoBehaviour, IAttacker
{
    [SerializeField] Transform temp;


    [SerializeField] Vector2 jumpForce;
    [SerializeField] float gravityScale;
    [SerializeField] float jumpDistance;

    bool hasAuthToRunScript;

    Rigidbody2D rb;
    Animator anim;
    AIStates aIStatesScript;

    const string SLIME_START_JUMP = "StartJump";
    const string SLIME_Mid_JUMP = "MidJump";
    const string SLIME_End_JUMP = "EndJump";

    float jumpTopThreshold;

    bool isStartOfJump;
    bool isMidJump;
    bool isEndOfJump;

    Vector2 targetPos;


    void Awake()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        anim = gameObject.GetComponent<Animator>();
        aIStatesScript = gameObject.GetComponent<AIStates>();

        jumpTopThreshold = jumpForce.y / 10;
    }

    void Update()
    {
        if (!hasAuthToRunScript) return;

        if (isStartOfJump && Mathf.Abs(rb.velocity.y) < jumpTopThreshold)
        {
            anim.Play(SLIME_Mid_JUMP);
            isStartOfJump = false;
            isMidJump = true;

        } else if (isMidJump && Mathf.Abs(targetPos.y  - transform.position.y) <= 0.3f)
        {
            anim.Play(SLIME_End_JUMP);
            isStartOfJump = false;
            isMidJump = false;
            isEndOfJump = true;
        }
    }

    public void StartAttack(Vector2 closestPlayer)
    {
        hasAuthToRunScript = true;
        targetPos = closestPlayer;
        StartJump();
        rb.gravityScale = 1;

        aIStatesScript.State = AIStates.States.Attacking;
    }

    public void EndAttack()
    {
        hasAuthToRunScript = false;
        rb.gravityScale = 0;

        aIStatesScript.State = AIStates.States.Idle;
    }

    private void StartJump()
    {
        anim.Play(SLIME_START_JUMP);
        isStartOfJump = true;

        // Vector2 playerDir = (targetPos - (Vector2)transform.position).normalized;
        // Vector2 landPoint = playerDir * jumpDistance + (Vector2)transform.position;
        // Instantiate(temp, landPoint, Quaternion.identity);
        rb.AddForce(Vector2.up * Mathf.Sqrt(-2.0f * Physics2D.gravity.y * jumpForce.y), ForceMode2D.Impulse);
    }
}
