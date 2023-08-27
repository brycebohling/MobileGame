using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class WallGenerator
{
    public static void CreateWalls(HashSet<Vector2Int> floorPositions, TilemapSpawner tilemapSpawnerScript)
    {
        var wallPositions = FindWallsInDirections(floorPositions, Direction2D.eightDirectionList);

        foreach (var position in wallPositions)
        {
            string neighbourBinaryType = "";

            foreach (var direction in Direction2D.eightDirectionList)
            {
                var neighbourPosition = position + direction;

                if (floorPositions.Contains(neighbourPosition))
                {
                    neighbourBinaryType += "1";

                } else
                {
                    neighbourBinaryType += "0";
                }
            }

            tilemapSpawnerScript.SpawnSingleWall(position, neighbourBinaryType);
        }
    }

    private static HashSet<Vector2Int> FindWallsInDirections(HashSet<Vector2Int> floorPositions, List<Vector2Int> directionsList)
    {
        HashSet<Vector2Int> wallPositions = new HashSet<Vector2Int>();

        foreach (var position in floorPositions)
        {
            foreach (var direction in directionsList)
            {
                var neighbourPosition = position + direction;

                if (!floorPositions.Contains(neighbourPosition))
                {
                    wallPositions.Add(neighbourPosition);
                }
            }
        }

        return wallPositions;
    }
}
