using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public static class ProceduralGenerationAlgorithms
{
    public static HashSet<Vector2Int> RandomWalk(Vector2Int startPosition, int walkLength, int walkWidth,
        int offset, int xMinBounds, int xMaxBounds, int yMinBounds, int yMaxBounds)
    {
        HashSet<Vector2Int> path = new()
        {
            startPosition
        };

        var previousPosition = startPosition;

        for (int i = 0; i < walkWidth; i++)
        {
            var currentUpPostion = previousPosition + Vector2Int.up * i;
            path.Add(currentUpPostion);

            for (int x = 1; x < walkWidth; x++)
            {
                path.Add(currentUpPostion + Vector2Int.right * x);    
            }   
        }

        for (int i = 0; i < walkLength / walkWidth; i++)
        {
            Vector2Int direction = Direction2D.GetRandomCardinalDirection();

            var newPosition = previousPosition + direction * walkWidth;
            
            if (newPosition.x >= (xMinBounds + offset) && newPosition.x <= (xMaxBounds - offset) && 
                newPosition.y >= (yMinBounds + offset) && newPosition.y <= (yMaxBounds - offset) &&
                newPosition.x + walkWidth >= (xMinBounds + offset) && newPosition.x + walkWidth<= (xMaxBounds - offset) && 
                newPosition.y + walkWidth>= (yMinBounds + offset) && newPosition.y + walkWidth<= (yMaxBounds - offset))
            {
                path.Add(newPosition);   

                for (int x = 0; x < walkWidth; x++)
                {
                    var currentUpPostion = newPosition + Vector2Int.up * x;
                    path.Add(currentUpPostion);

                    for (int y = 1; y < walkWidth; y++)
                    {
                        path.Add(currentUpPostion + Vector2Int.right * y);    
                    }   
                }

                previousPosition = newPosition;

            }
        }

        return path;
    }

    public static List<Vector2Int> RandomWalkCorridor(Vector2Int startPosition, int corridorLength, int corridorWidth)
    {
        List<Vector2Int> corridor = new()
        {
            startPosition
        };

        var direction = Direction2D.GetRandomCardinalDirection();
        var newPosition = startPosition;

        List<Vector2Int> sideDirections = GetSideDirections(direction);

        for (int i = 2; i < corridorWidth + 1; i++)
        {
            corridor.Add(newPosition + sideDirections[i % 2] * i / 2);
        }
        
        for (int i = 0; i < corridorLength; i++)
        {
            newPosition += direction;
            corridor.Add(newPosition);

            for (int y = 2; y < corridorWidth + 1; y++)
            {
                corridor.Add(newPosition + sideDirections[y % 2] * y / 2);    
            }
        }

        return corridor;
    }

    public static List<BoundsInt> BinarySpacePartitioning(BoundsInt spaceToSplit, int maxWidth, int maxHeight)
    {
        Queue<BoundsInt> roomsQueue = new();
        List<BoundsInt> roomsList = new();
        roomsQueue.Enqueue(spaceToSplit);
        
        while (roomsQueue.Count > 0)
        {
            var room = roomsQueue.Dequeue();

            if (room.size.y >= maxHeight && room.size.x >= maxWidth)
            {
                if (Random.value < 0.5f)
                {
                    if (room.size.y >= maxHeight * 2)
                    {
                        SplitHorizontally(roomsQueue, room);

                    } else if (room.size.x >= maxWidth * 2)
                    {
                        SplitVertically(roomsQueue, room);

                    } else
                    {
                        roomsList.Add(room);
                    }

                } else
                {
                    if (room.size.x >= maxWidth * 2)
                    {
                        SplitVertically(roomsQueue, room);

                    } else if (room.size.y >= maxHeight * 2)
                    {
                        SplitHorizontally(roomsQueue, room);

                    } else
                    {
                        roomsList.Add(room);
                    }
                }
            }
        }

        return roomsList;
    }

    private static void SplitVertically(Queue<BoundsInt> roomsQueue, BoundsInt room)
    {
        var xSplit = Random.Range(1, room.size.x);
        BoundsInt room1 = new (room.min, new Vector3Int(xSplit, room.size.y, room.size.z));
        BoundsInt room2 = new (new Vector3Int(room.min.x + xSplit, room.min.y, room.min.z),
            new Vector3Int(room.size.x - xSplit, room.size.y, room.size.z));
        roomsQueue.Enqueue(room1);
        roomsQueue.Enqueue(room2);
    }

    private static void SplitHorizontally(Queue<BoundsInt> roomsQueue, BoundsInt room)
    {
        var ySplit = Random.Range(1, room.size.y);
        BoundsInt room1 = new (room.min, new Vector3Int(room.size.x, ySplit, room.min.z));
        BoundsInt room2 = new (new Vector3Int(room.min.x, room.min.y + ySplit, room.min.z),
            new Vector3Int(room.size.x, room.size.y - ySplit, room.size.z));
        roomsQueue.Enqueue(room1);
        roomsQueue.Enqueue(room2);
    }

    private static List<Vector2Int> GetSideDirections(Vector2Int direction)
    {
        if (direction == Vector2Int.up)
        {
            return new List<Vector2Int> { Vector2Int.right, Vector2Int.left };
        }

        if (direction == Vector2Int.right)
        {
            return new List<Vector2Int> { Vector2Int.up, Vector2Int.down };
        }

        if (direction == Vector2Int.down)
        {
            return new List<Vector2Int> { Vector2Int.right, Vector2Int.left };
        }

        if (direction == Vector2Int.left)
        {
            return new List<Vector2Int> { Vector2Int.up, Vector2Int.down };
        }

        Debug.Log("Error");
        return new List<Vector2Int> { Vector2Int.zero, Vector2Int.zero };
    }
}