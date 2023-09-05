using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class RandomWalkMapGenerator : AbstractDungeonGenerator
{
    [SerializeField] protected RandomWalkSO randomWalkParameters;


    protected override void RunProceduralGeneration()
    {
        // HashSet<Vector2Int> floorPositions = RunRandomWalk(randomWalkParameters, startPosition);

        // tilemapSpawnerScript.Clear();
        // tilemapSpawnerScript.SpawnFloorTiles(floorPositions);
        // WallGenerator.CreateWalls(floorPositions, tilemapSpawnerScript);
    }

    protected HashSet<Vector2Int> RunRandomWalk(RandomWalkSO paramenters, Vector2Int position,
        int offset, int xMinBounds, int xMaxBounds, int yMinBounds, int yMaxBounds)
    {
        var currentPostion = position;
        HashSet<Vector2Int> floorPositions = new();
        
        for (int i = 0; i < paramenters.iteration; i++)
        {
            var path = ProceduralGenerationAlgorithms.RandomWalk(currentPostion, paramenters.walkLength, paramenters.walkWidth,
                offset, xMinBounds, xMaxBounds, yMinBounds, yMaxBounds);
            floorPositions.UnionWith(path);

            if (paramenters.startRandomPosEachIteration)
            {
                currentPostion = floorPositions.ElementAt(Random.Range(0, floorPositions.Count));
            }
        }

        return floorPositions;
    }
}
