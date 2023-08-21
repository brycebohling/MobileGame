using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStates : MonoBehaviour
{
    public enum States
    {
        Walking,
        Idle,
        Dashing,
        Attacking,
        Damaged,
    }

    public PlayerStates.States State;
    public AnimationClip baseAnimationClip;
}
