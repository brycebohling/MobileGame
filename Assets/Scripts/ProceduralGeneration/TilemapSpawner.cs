using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapSpawner : MonoBehaviour
{
    [SerializeField] Tilemap floorTilemap;
    [SerializeField] Tilemap wallTilemap;

    [SerializeField] TileBase floorTile;
    [SerializeField] TileBase wallFull;
    [SerializeField] TileBase wallTop;
    [SerializeField] TileBase wallBottom;
    [SerializeField] TileBase wallSideRight;
    [SerializeField] TileBase wallSideLeft;
    [SerializeField] TileBase wallInnerCornerDownRight;
    [SerializeField] TileBase wallInnerCornerDownLeft;
    [SerializeField] TileBase wallInnerCornerTopRight;
    [SerializeField] TileBase wallInnerCornerTopLeft;
    [SerializeField] TileBase wallOuterCornerDownRight;
    [SerializeField] TileBase wallOuterCornerDownLeft;
    [SerializeField] TileBase wallOuterCornerUpRight;
    [SerializeField] TileBase wallOuterCornerUpLeft;
    [SerializeField] TileBase wallFillRight;
    [SerializeField] TileBase wallFillLeft;
    

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
        wallTilemap.ClearAllTiles();
    }

    internal void SpawnSingleWall(Vector2Int position, string binaryType)
    {
        int typeAsInt = Convert.ToInt32(binaryType, 2);
        TileBase tile = null;

        Debug.Log(position + "       " + binaryType);
        
        if (WallTypesHelper.wallTop.Contains(typeAsInt))
        {
            tile = wallTop;

        } else if (WallTypesHelper.wallSideRight.Contains(typeAsInt))
        {
            tile = wallSideRight;

        } else if (WallTypesHelper.wallSideLeft.Contains(typeAsInt))
        {
            tile = wallSideLeft;

        } else if (WallTypesHelper.wallBottom.Contains(typeAsInt))
        {
            tile = wallBottom;

        } else if (WallTypesHelper.wallFull.Contains(typeAsInt))
        {
            tile = wallFull;

        } else if (WallTypesHelper.wallInnerCornerDownRight.Contains(typeAsInt))
        {
            tile = wallInnerCornerDownRight;

        } else if (WallTypesHelper.wallInnerCornerDownLeft.Contains(typeAsInt))
        {
            tile = wallInnerCornerDownLeft;

        } else if (WallTypesHelper.wallInnerCornerTopRight.Contains(typeAsInt))
        {
            tile = wallInnerCornerTopRight;

        } else if (WallTypesHelper.wallInnerCornerTopLeft.Contains(typeAsInt))
        {
            tile = wallInnerCornerTopLeft;

        } else if (WallTypesHelper.wallOuterCornerDownRight.Contains(typeAsInt))
        {
            tile = wallOuterCornerDownRight;

        } else if (WallTypesHelper.wallOuterCornerDownLeft.Contains(typeAsInt))
        {
            tile = wallOuterCornerDownLeft;

        } else if (WallTypesHelper.wallOuterCornerUpRight.Contains(typeAsInt))
        {
            tile = wallOuterCornerUpRight;

        } else if (WallTypesHelper.wallOuterCornerUpLeft.Contains(typeAsInt))
        {
            tile = wallOuterCornerUpLeft;
        } else if (WallTypesHelper.wallFillRight.Contains(typeAsInt))
        {
            tile = wallFillRight;

        } else if (WallTypesHelper.wallFillLeft.Contains(typeAsInt))
        {
            tile = wallFillLeft;
        }

        if (tile != null)
        {
            SpawnSingleTile(wallTilemap, tile, position);
        }
    }
}
