using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RandomWalkParaments_", menuName = "PCG/RandomWalkData")]
public class RandomWalkSO : ScriptableObject
{
    public int iteration = 10;
    public int walkLength = 10;
    public bool startRandomlyEachIteration = true;
    
}
