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
    }

    public enum CharacterConditions
    {
        Walking,
        Idle,
        Dashing,
    }

    public CharacterStates.MovementStates _movementState;
    public CharacterStates.CharacterConditions _characterConditionState;
}
