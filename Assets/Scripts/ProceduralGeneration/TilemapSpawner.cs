using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapSpawner : MonoBehaviour
{
    [SerializeField] private Tilemap floorTilemap;

    [SerializeField] private TileBase floorTile;

    public void SpawnFloorTiles(IEnumerable<Vector2Int> floorPosition)
    {
        SpawnTiles(floorPosition, floorTilemap, floorTile);
    }

    private void SpawnTiles(IEnumerable<Vector2Int> postions, Tilemap tilemap, TileBase tile)
    {
        foreach (var position in postions)
        {
            SpawnSingleTile(tilemap, tile, position);
        }
    }

    private void SpawnSingleTile(Tilemap tilemap, TileBase tile, Vector2Int position)
    {
        var tilePostion = tilemap.WorldToCell((Vector3Int)position);
        tilemap.SetTile(tilePostion, tile);
    }

    public void Clear()
    {
        floorTilemap.ClearAllTiles();
    }
}
