using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStates : MonoBehaviour
{
    public enum States
    {
        Walking,
        Idle,
        Dashing,
        Attacking,
        Damaged,
    }

    public CharacterStates.States State;
    public AnimationClip baseAnimationClip;
}
