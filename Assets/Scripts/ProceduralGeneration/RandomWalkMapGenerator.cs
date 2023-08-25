using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class RandomWalkMapGenerator : MonoBehaviour
{
    [SerializeField] protected Vector2Int startPosition = Vector2Int.zero;

    [SerializeField] private int iteration = 10;
    public int walkLength = 10;
    public bool startRandomlyEachIteration = true;

    public void RunProceduralGeneration()
    {
        HashSet<Vector2Int> floorPositions = RunRandomWalk();
        
        foreach (var position in floorPositions)
        {
            Debug.Log(position);
        }
    }

    protected HashSet<Vector2Int> RunRandomWalk()
    {
        var currentPostion = startPosition;
        HashSet<Vector2Int> floorPositions = new HashSet<Vector2Int>();

        for (int i = 0; i < iteration; i++)
        {
            var path = ProceduralGenerationAlgorithms.RandomWalk(currentPostion, walkLength);
            floorPositions.UnionWith(path);

            if (startRandomlyEachIteration)
            {
                currentPostion = floorPositions.ElementAt(Random.Range(0, floorPositions.Count));
            }
        }

        return floorPositions;
    }
}
