using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIStates : MonoBehaviour
{
    public enum States
    {
        Patrolling,
        Idle,
        Attacking,
        Chasing,
        Damaged,
        Dead,
    }

    public States State;

    public AnimationClip baseAnimationClip;
}
