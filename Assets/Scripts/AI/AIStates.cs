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
    }

    public States State;
}
