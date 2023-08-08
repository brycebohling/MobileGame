using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIAttack : AIBase
{
    public AIStates.States[] BlockingActionStates;
    [SerializeField] float dmg;
    [SerializeField] float tbAttacks;


    protected override void Start()
    {
        base.Start();
    }

    void Update()
    {
        if (!IsActionAuth(BlockingActionStates)) return;

        HandleAction();
    }

    protected override bool IsActionAuth(AIStates.States[] blockingActionStates)
    {
        return base.IsActionAuth(blockingActionStates);
    }

    protected override void HandleAction()
    {
        // if player is in range attack
    }
}
