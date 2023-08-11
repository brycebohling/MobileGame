using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(AIStates))]
public class SlimeAttack : MonoBehaviour, IAttacker
{
    [SerializeField] Transform temp;

    [SerializeField] Animator bodyAnim;
    [SerializeField] Vector2 jumpForce;
    [SerializeField] float gravityScale;
    [SerializeField] float jumpDistance;
    
    bool hasAuthToRunScript;

    Rigidbody2D rb;
    AIStates aIStatesScript;

    const string SLIME_START_JUMP = "StartJump";
    const string SLIME_Mid_JUMP = "MidJump";
    const string SLIME_End_JUMP = "EndJump";

    bool isStartOfJump;
    bool isMidJump;
    bool isEndOfJump;

    Vector2 targetPos;


    void Awake()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        aIStatesScript = gameObject.GetComponent<AIStates>();
    }

    void Update()
    {
        if (!hasAuthToRunScript) return;

    }

    public void StartAttack(Vector2 closestPlayer)
    {
        hasAuthToRunScript = true;
        targetPos = closestPlayer;
        StartJump();
        rb.gravityScale = gravityScale;

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
        Helpers.ChangeAnimationState(bodyAnim, SLIME_START_JUMP);
        isStartOfJump = true;

        Vector2 playerDir = (targetPos - (Vector2)transform.position).normalized;
        Vector2 landPoint = playerDir * jumpDistance + (Vector2)transform.position;

        Instantiate(temp, landPoint, Quaternion.identity);

        // control point in mid of jump

        rb.velocity = new Vector2(playerDir.x * jumpForce.x, Mathf.Sqrt(-2.0f * Physics2D.gravity.y * jumpForce.y));
    }
}