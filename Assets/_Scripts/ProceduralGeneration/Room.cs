using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room
{
    public Vector2 RoomCenterPos { get; set; }
    public HashSet<Vector2Int> FloorTiles { get; private set; } = new();

    public HashSet<Vector2Int> NearWallTilesUp { get; private set; } = new();
    public HashSet<Vector2Int> NearWallTilesDown { get; private set; } = new();
    public HashSet<Vector2Int> NearWallTilesRight { get; private set; } = new();
    public HashSet<Vector2Int> NearWallTilesLeft { get; private set; } = new();
    public HashSet<Vector2Int> CornerTiles { get; private set; } = new();
    public HashSet<Vector2Int> InnerTiles { get; private set; } = new();

    public HashSet<Vector2Int> PropPositions { get; private set; } = new();
    public HashSet<GameObject> PropObjectReference { get; private set; } = new();
    public HashSet<Vector2Int> PositionsAccessibleFromPath { get; private set; } = new();
    public HashSet<GameObject> EnemiesInRoom { get; private set; } = new();

    public Room(Vector2 roomCenterPos, HashSet<Vector2Int>  floorTiles)
    {
        this.RoomCenterPos = roomCenterPos;
        this.FloorTiles = floorTiles;
    }
}
