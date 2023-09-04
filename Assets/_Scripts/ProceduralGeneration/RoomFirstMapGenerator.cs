using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using UnityEngine.Events;

public class RoomFirstMapGenerator : RandomWalkMapGenerator
{
    [SerializeField] int maxRoomWidth = 4;
    [SerializeField] int maxRoomHeight = 4;
    [SerializeField] int dungeonWidth = 20;
    [SerializeField] int dungeonHeight = 20;
    [SerializeField] [Range(0, 10)] int offset = 1;
    [SerializeField] bool randomWalkRooms = false;
    [SerializeField] bool debugCreationTime;
    
    public UnityEvent OnDungeonLayoutGenerated;

    float startCreationTime;
    


    protected override void RunProceduralGeneration()
    {
        startCreationTime = Time.realtimeSinceStartup;

        CreateRooms();

        OnDungeonLayoutGenerated?.Invoke();
    }

    public void DebugDungeonCreationTime()
    {
        if (!debugCreationTime) return;
        
        Debug.Log("Created in " + Mathf.Round((Time.realtimeSinceStartup - startCreationTime) * 1000f) + "ms");
    }

    private void CreateRooms()
    {
        var roomsList = ProceduralGenerationAlgorithms.BinarySpacePartitioning(new BoundsInt((Vector3Int)startPosition, new Vector3Int(dungeonWidth, dungeonHeight)),
            maxRoomWidth, maxRoomHeight);
        
        HashSet<Vector2Int> floor = new();

        if (randomWalkRooms)
        {
            floor = CreateRoomsRandomly(roomsList);
        } else
        {
            floor = CreateSimpleRooms(roomsList);
        }

        List<Vector2Int> roomCenter = new();

        foreach (var room in roomsList)
        {
            roomCenter.Add((Vector2Int)Vector3Int.RoundToInt(room.center));
        }

        HashSet<Vector2Int> corridors = ConnectRooms(roomCenter);
        floor.UnionWith(corridors);

        tilemapSpawnerScript.SpawnFloorTiles(floor);
        tilemapSpawnerScript.SpawnCorridorTile(corridors);
        WallGenerator.CreateWalls(floor, tilemapSpawnerScript);
    }

    private HashSet<Vector2Int> CreateRoomsRandomly(List<BoundsInt> roomsList)
    {
        HashSet<Vector2Int> floor = new();

        for (int i = 0; i < roomsList.Count; i++)
        {
            var roomBounds = roomsList[i];
            var roomCenter = new Vector2Int(Mathf.RoundToInt(roomBounds.center.x), Mathf.RoundToInt(roomBounds.center.y));

            var roomFloor = RunRandomWalk(randomWalkParameters, roomCenter,
                offset, roomBounds.xMin, roomBounds.xMax, roomBounds.yMin, roomBounds.yMax);


            foreach (var position in roomFloor)
            {
                floor.Add(position);
            }
        }

        return floor;
    }

    private HashSet<Vector2Int> ConnectRooms(List<Vector2Int> roomCenters)
    {
        HashSet<Vector2Int> corridors = new();
        var currentRoomCenter = roomCenters[Random.Range(0, roomCenters.Count)];
        roomCenters.Remove(currentRoomCenter);

        while (roomCenters.Count > 0)
        {
            Vector2Int closest = FindClosestPointTo(currentRoomCenter, roomCenters);
            roomCenters.Remove(closest);
            HashSet<Vector2Int> newCorridor = CreateCorridor(currentRoomCenter, closest);
            currentRoomCenter = closest;
            corridors.UnionWith(newCorridor);
        }

        return corridors;
    }

    private HashSet<Vector2Int> CreateCorridor(Vector2Int currentRoomCenter, Vector2Int destination)
    {   
        HashSet<Vector2Int> corridor = new();
        var position = currentRoomCenter;
        corridor.Add(position);

        while (position.y != destination.y)
        {
            if (destination.y > position.y)
            {
                position += Vector2Int.up;

            } else if (destination.y < position.y)
            {
                position += Vector2Int.down;
            }

            corridor.Add(position);
        }

        while (position.x != destination.x)
        {
            if (destination.x > position.x)
            {
                position += Vector2Int.right;

            } else if (destination.x < position.x)
            {
                position += Vector2Int.left;
            }

            corridor.Add(position);
        }

        return corridor;
    }

    private Vector2Int FindClosestPointTo(Vector2Int currentRoomCenter, List<Vector2Int> roomCenters)
    {
        Vector2Int closest = Vector2Int.zero;
        float distance = float.MaxValue;

        foreach (var position in roomCenters)
        {
            float currentDistance = Vector2.Distance(position, currentRoomCenter);

            if (currentDistance < distance)
            {
                distance = currentDistance;
                closest = position;
            }
        }

        return closest;
    }

    private HashSet<Vector2Int> CreateSimpleRooms(List<BoundsInt> roomsList)
    {
        HashSet<Vector2Int> floor = new();

        foreach (var room in roomsList)
        {
            for (int col = offset; col < room.size.x - offset; col++)
            {
                for (int row = offset; row < room.size.y - offset; row++)
                {
                    Vector2Int position = (Vector2Int)room.min + new Vector2Int(col, row);
                    floor.Add(position);
                }
            }
        }

        return floor;
    }
}
