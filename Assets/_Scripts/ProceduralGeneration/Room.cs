using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room
{
    public int RoomDifficulty;

    public Vector2Int RoomCenterPos { get; set; }
    public HashSet<Vector2Int> FloorTiles { get; private set; } = new();
    public HashSet<Vector2Int> ClearFloorTiles { get; private set; } = new();

    public HashSet<Vector2Int> NearWallTilesUp { get; private set; } = new();
    public HashSet<Vector2Int> NearWallTilesDown { get; private set; } = new();
    public HashSet<Vector2Int> NearWallTilesRight { get; private set; } = new();
    public HashSet<Vector2Int> NearWallTilesLeft { get; private set; } = new();
    public HashSet<Vector2Int> CornerTiles { get; private set; } = new();
    public HashSet<Vector2Int> InnerTiles { get; private set; } = new();

    public HashSet<Vector2Int> PropPositions { get; private set; } = new();
    public HashSet<Transform> PropTransfromReference { get; private set; } = new();
    public HashSet<Vector2Int> EnemyPositions { get; private set; } = new();
    public HashSet<Transform> EnemyTransfromReference { get; private set; } = new();

    public Room(Vector2Int roomCenterPos, HashSet<Vector2Int>  floorTiles, int roomDifficulty)
    {
        RoomCenterPos = roomCenterPos;
        FloorTiles = floorTiles;
        RoomDifficulty = roomDifficulty;
    }
}
