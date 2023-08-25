using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class RandomWalkMapGenerator : AbstractDungeonGenerator
{
    [SerializeField] private RandomWalkSO randomWalkParameters;

    protected override void RunProceduralGeneration()
    {
        HashSet<Vector2Int> floorPositions = RunRandomWalk();

        tilemapSpawnerScript.Clear();
        tilemapSpawnerScript.SpawnFloorTiles(floorPositions);
    }

    protected HashSet<Vector2Int> RunRandomWalk()
    {
        var currentPostion = startPosition;
        HashSet<Vector2Int> floorPositions = new HashSet<Vector2Int>();

        for (int i = 0; i < randomWalkParameters.iteration; i++)
        {
            var path = ProceduralGenerationAlgorithms.RandomWalk(currentPostion, randomWalkParameters.walkLength);
            floorPositions.UnionWith(path);

            if (randomWalkParameters.startRandomlyEachIteration)
            {
                currentPostion = floorPositions.ElementAt(Random.Range(0, floorPositions.Count));
            }
        }

        return floorPositions;
    }
}
