using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapSpawner : MonoBehaviour
{
    [SerializeField] private Tilemap floorTilemap;
    [SerializeField] private Tilemap wallTilemap;

    [SerializeField] private TileBase floorTile;
    [SerializeField] private TileBase wallFull;
    [SerializeField] private TileBase wallTop;
    [SerializeField] private TileBase wallBottom;
    [SerializeField] private TileBase wallSideRight;
    [SerializeField] private TileBase wallSideLeft;
    [SerializeField] private TileBase wallInnerCornerDownRight;
    [SerializeField] private TileBase wallInnerCornerDownLeft;
    [SerializeField] private TileBase wallOuterCornerDownRight;
    [SerializeField] private TileBase wallOuterCornerDownLeft;
    [SerializeField] private TileBase wallOuterCornerUpRight;
    [SerializeField] private TileBase wallOuterCornerUpLeft;
    

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

    // internal void SpawnSingleBasicWall(Vector2Int position, string binaryType)
    // {
    //     int typeAsInt = Convert.ToInt32(binaryType, 2);
    //     TileBase tile = null;
        
    //     if (WallTypesHelper.wallTop.Contains(typeAsInt))
    //     {
    //         tile = wallTop;

    //     } else if (WallTypesHelper.wallSideRight.Contains(typeAsInt))
    //     {
    //         tile = wallSideRight;

    //     } else if (WallTypesHelper.wallSideLeft.Contains(typeAsInt))
    //     {
    //         tile = wallSideLeft;

    //     } else if (WallTypesHelper.wallBottom.Contains(typeAsInt))
    //     {
    //         tile = wallBottom;

    //     } else if (WallTypesHelper.wallFull.Contains(typeAsInt))
    //     {
    //         tile = wallFull;
    //     }

    //     if (tile != null)
    //     {
    //         SpawnSingleTile(wallTilemap, tile, position);
    //     }
    // }

    // internal void SpawnSingleCornerWall(Vector2Int position, string binaryType)
    // {
    //     int typeAsInt = Convert.ToInt32(binaryType, 2);
    //     TileBase tile = null;

    //     if (WallTypesHelper.wallInnerCornerDownRight.Contains(typeAsInt))
    //     {
    //         tile = wallInnerCornerDownRight;

    //     } else if (WallTypesHelper.wallInnerCornerDownLeft.Contains(typeAsInt))
    //     {
    //         tile = wallInnerCornerDownLeft;

    //     } else if (WallTypesHelper.wallOuterCornerDownRight.Contains(typeAsInt))
    //     {
    //         tile = wallOuterCornerDownRight;

    //     } else if (WallTypesHelper.wallOuterCornerDownLeft.Contains(typeAsInt))
    //     {
    //         tile = wallOuterCornerDownLeft;

    //     } else if (WallTypesHelper.wallOuterCornerUpRight.Contains(typeAsInt))
    //     {
    //         tile = wallOuterCornerUpRight;

    //     } else if (WallTypesHelper.wallOuterCornerUpLeft.Contains(typeAsInt))
    //     {
    //         tile = wallOuterCornerUpLeft;

    //     } else if (WallTypesHelper.wallFullEightDirections.Contains(typeAsInt))
    //     {
    //         tile = wallFull;

    //     } else if (WallTypesHelper.wallBottmEightDirections.Contains(typeAsInt))
    //     {
    //         tile = wallBottom;
    //     } 

    //     if (tile != null)
    //     {
    //         SpawnSingleTile(wallTilemap, tile, position);
    //     }

    // }

    internal void SpawnSingleWall(Vector2Int position, string binaryType)
    {
        int typeAsInt = Convert.ToInt32(binaryType, 2);
        TileBase tile = null;
        
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

        } else if (WallTypesHelper.wallFullEightDirections.Contains(typeAsInt))
        {
            tile = wallFull;

        } else if (WallTypesHelper.wallBottmEightDirections.Contains(typeAsInt))
        {
            tile = wallBottom;
        } 

        if (tile != null)
        {
            SpawnSingleTile(wallTilemap, tile, position);
        }
    }
}
