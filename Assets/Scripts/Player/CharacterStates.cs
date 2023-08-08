using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStates : MonoBehaviour
{
    public enum MovementStates
    {
        Walking,
        Idle,
        Dashing,
        Attacking,
        Partolling,
    }

    public CharacterStates.MovementStates MovementState;
}
