using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using UnityEngine.Events;
using System.Linq;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class DungeonGenerator : MonoBehaviour
{
    public UnityEvent OnDungeonLayoutGenerated;
    public UnityEvent OnFinishedDungeonGeneration;

    [SerializeField] TilemapSpawner  tilemapSpawnerScript = null;
    [SerializeField] AStarGridSettings aStarGridSettings;
    [SerializeField] Vector2Int startPosition = Vector2Int.zero;

    [SerializeField] RandomWalkSO randomWalkParameters;

    [SerializeField] int maxRoomWidth = 4;
    [SerializeField] int maxRoomHeight = 4;
    public int dungeonWidth = 20;
    public int dungeonHeight = 20;
    [SerializeField] [Range(0, 10)] int offset = 1;
    [SerializeField] bool randomWalkRooms = false;
    [SerializeField] bool debugCreationTime;

    public List<Room> RoomList = new();
    public HashSet<Vector2Int> Path = new();
    float startCreationTime;


    private void Start() 
    {
        GenerateDungeon();    
    }

    public void GenerateDungeon()
    {
        ClearDungeon();
        RunProceduralGeneration();
    }

    public void ClearDungeon()
    {
        tilemapSpawnerScript.Clear();
        aStarGridSettings.DestroyGrid();
        ClearRooms();
    }

    private void RunProceduralGeneration()
    {
        startCreationTime = Time.realtimeSinceStartup;

        CreateRooms();

        OnDungeonLayoutGenerated?.Invoke();
    }

    private void ClearRooms()
    {
        tilemapSpawnerScript.Clear();

        foreach (Room room in RoomList)
        {   
            foreach (Transform prop in room.PropTransfromReference)
            {
                if (prop != null) DestroyImmediate(prop.gameObject);
            }

            foreach (Transform enemy in room.EnemyTransfromReference)
            {
                if (enemy != null) DestroyImmediate(enemy.gameObject);
            }
        }

        RoomList.Clear();
    }

    private void CreateRooms()
    {
        var roomAreas = ProceduralGenerationAlgorithms.BinarySpacePartitioning(new BoundsInt((Vector3Int)startPosition, new Vector3Int(dungeonWidth, dungeonHeight)),
            maxRoomWidth, maxRoomHeight);
        
        HashSet<Vector2Int> floor = new();

        if (randomWalkRooms)
        {
            floor = CreateRoomsRandomly(roomAreas);
        } else
        {
            floor = CreateSimpleRooms(roomAreas);
        }

        List<Vector2Int> roomCenters = new();

        foreach (var room in roomAreas)
        {
            roomCenters.Add((Vector2Int)Vector3Int.RoundToInt(room.center));
        }

        Path = ConnectRooms(roomCenters);

        floor.UnionWith(Path);

        tilemapSpawnerScript.SpawnFloorTiles(floor);
        tilemapSpawnerScript.SpawnCorridorTile(Path);
        WallGenerator.CreateWalls(floor, tilemapSpawnerScript);
    }

    private HashSet<Vector2Int> CreateRoomsRandomly(List<BoundsInt> roomAreas)
    {
        HashSet<Vector2Int> floor = new();

        for (int i = 0; i < roomAreas.Count; i++)
        {
            var roomBounds = roomAreas[i];
            var roomCenter = new Vector2Int(Mathf.RoundToInt(roomBounds.center.x), Mathf.RoundToInt(roomBounds.center.y));

            var roomFloor = RunRandomWalk(randomWalkParameters, roomCenter,
                offset, roomBounds.xMin, roomBounds.xMax, roomBounds.yMin, roomBounds.yMax);

            Room room = new(roomCenter, roomFloor);

            RoomList.Add(room);

            foreach (var position in roomFloor)
            {
                floor.Add(position);    
            }
        }
        
        return floor;
    }

    private HashSet<Vector2Int> CreateSimpleRooms(List<BoundsInt> roomAreas)
    {
        HashSet<Vector2Int> floor = new();

        foreach (var area in roomAreas)
        {
            HashSet<Vector2Int> roomFloor = new();
            var roomBounds = area;
            var roomCenter = new Vector2Int(Mathf.RoundToInt(roomBounds.center.x), Mathf.RoundToInt(roomBounds.center.y));

            for (int col = offset; col < area.size.x - offset; col++)
            {
                for (int row = offset; row < area.size.y - offset; row++)
                {
                    Vector2Int position = (Vector2Int)area.min + new Vector2Int(col, row);

                    roomFloor.Add(position);

                    floor.Add(position);
                }
            }

            Room room = new(roomCenter, roomFloor);
            RoomList.Add(room);
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
        List<Vector2Int> sideDirections = new();

        while (position.y != destination.y)
        {
            if (destination.y > position.y)
            {
                position += Vector2Int.up;
                sideDirections = ProceduralGenerationAlgorithms.GetSideDirections(Vector2Int.up);

            } else if (destination.y < position.y)
            {
                position += Vector2Int.down;
                sideDirections = ProceduralGenerationAlgorithms.GetSideDirections(Vector2Int.down);
            }

            corridor.Add(position);

            for (int y = 2; y < randomWalkParameters.walkWidth + 1; y++)
            {
                corridor.Add(position + sideDirections[y % 2] * y / 2);    
            }
        }

        while (position.x != destination.x)
        {
            if (destination.x > position.x)
            {
                position += Vector2Int.right;
                sideDirections = ProceduralGenerationAlgorithms.GetSideDirections(Vector2Int.right);

            } else if (destination.x < position.x)
            {
                position += Vector2Int.left;
                sideDirections = ProceduralGenerationAlgorithms.GetSideDirections(Vector2Int.left);
            }

            corridor.Add(position);

            for (int y = 2; y < randomWalkParameters.walkWidth + 1; y++)
            {
                corridor.Add(position + sideDirections[y % 2] * y / 2);    
            }
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

    private HashSet<Vector2Int> RunRandomWalk(RandomWalkSO paramenters, Vector2Int position,
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

    public void BrodcastDungeonComplete()
    {
        OnFinishedDungeonGeneration?.Invoke();
        
        if (!debugCreationTime) return;
        
        Debug.Log("Created in " + Mathf.Round((Time.realtimeSinceStartup - startCreationTime) * 1000f) + "ms");
    }

    public void PlacePlayer()
    {
        List<int> furthestRoomIndexList = FindFurthestRooms();

        GameManager.Gm.playerTransfrom.position = (Vector2)RoomList[furthestRoomIndexList[0]].RoomCenterPos;
    }

    private List<int> FindFurthestRooms()
    {   
        float furthestDistance = 0;
        List<int> roomIndex = new();

        for (int i = 0; i < RoomList.Count; i++)
        {
            for (int x = 0; x < RoomList.Count; x++)
            {
                float distance = Vector2.Distance(RoomList[i].RoomCenterPos, RoomList[x].RoomCenterPos);
                if (distance > furthestDistance)
                {
                    furthestDistance = distance;
                    roomIndex.Clear();
                    roomIndex = new() {i, x};
                }
            }
        }

        return roomIndex;
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(DungeonGenerator))]
public class DungeonGeneratorEditor : Editor
{
	public override void OnInspectorGUI()
	{
        base.OnInspectorGUI();

		DungeonGenerator dungeonGenerator = (DungeonGenerator)target;

        if (GUILayout.Button("Create Dungeon"))
        {
            dungeonGenerator.GenerateDungeon();
        }

        if (GUILayout.Button("Clear Dungeon"))
        {
            dungeonGenerator.ClearDungeon();
        }
	}
}
#endif
