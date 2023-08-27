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
    [SerializeField] TileBase wallMain;
    [SerializeField] TileBase wallSideRight;
    [SerializeField] TileBase wallSideLeft;
    [SerializeField] TileBase wallInnerCornerDownRight;
    [SerializeField] TileBase wallInnerCornerDownLeft;
    [SerializeField] TileBase wallInnerCornerTopRight;
    [SerializeField] TileBase wallInnerCornerTopLeft;
    [SerializeField] TileBase wallOuterCornerDownRight;
    [SerializeField] TileBase wallOuterCornerDownLeft;
    [SerializeField] TileBase wallOuterCornerTopRight;
    [SerializeField] TileBase wallOuterCornerTopLeft;
    [SerializeField] TileBase wallFill;
    [SerializeField] TileBase wallFillRight;
    [SerializeField] TileBase wallFillLeft;
    [SerializeField] TileBase wallFillTop;
    [SerializeField] TileBase wallFillDown;
    [SerializeField] TileBase wallDoubleCenter;
    [SerializeField] TileBase wallDoubleTopRL;
    [SerializeField] TileBase wallDoubleTopR;
    [SerializeField] TileBase wallDoubleTopL;
    [SerializeField] TileBase wallDoubleDownRL;
    [SerializeField] TileBase wallDoubleDownR;
    [SerializeField] TileBase wallDoubleDownL;
    
    

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
        
        if (WallTypesHelper.wallMain.Contains(typeAsInt))
        {
            tile = wallMain;

        } else if (WallTypesHelper.wallSideRight.Contains(typeAsInt))
        {
            tile = wallSideRight;

        } else if (WallTypesHelper.wallSideLeft.Contains(typeAsInt))
        {
            tile = wallSideLeft;

        } else if (WallTypesHelper.wallFill.Contains(typeAsInt))
        {
            tile = wallFill;

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

        } else if (WallTypesHelper.wallOuterCornerTopRight.Contains(typeAsInt))
        {
            tile = wallOuterCornerTopRight;

        } else if (WallTypesHelper.wallOuterCornerTopLeft.Contains(typeAsInt))
        {
            tile = wallOuterCornerTopLeft;

        } else if (WallTypesHelper.wallFillRight.Contains(typeAsInt))
        {
            tile = wallFillRight;

        } else if (WallTypesHelper.wallFillLeft.Contains(typeAsInt))
        {
            tile = wallFillLeft;
            
        } else if (WallTypesHelper.wallFillTop.Contains(typeAsInt))
        {
            tile = wallFillTop;

        } else if (WallTypesHelper.wallFillDown.Contains(typeAsInt))
        {
            tile = wallFillDown;

        } else if (WallTypesHelper.wallDoubleCenter.Contains(typeAsInt))
        {
            tile = wallDoubleCenter;

        } else if (WallTypesHelper.wallDoubleTopRL.Contains(typeAsInt))
        {
            tile = wallDoubleTopRL;

        } else if (WallTypesHelper.wallDoubleTopR.Contains(typeAsInt))
        {
            tile = wallDoubleTopR;

        } else if (WallTypesHelper.wallDoubleTopL.Contains(typeAsInt))
        {
            tile = wallDoubleTopL;

        } else if (WallTypesHelper.wallDoubleDownRL.Contains(typeAsInt))
        {
            tile = wallDoubleDownRL;

        } else if (WallTypesHelper.wallDoubleDownR.Contains(typeAsInt))
        {
            tile = wallDoubleDownR;

        } else if (WallTypesHelper.wallDoubleDownL.Contains(typeAsInt))
        {
            tile = wallDoubleDownL;

        }

        if (tile != null)
        {
            SpawnSingleTile(wallTilemap, tile, position);

        } else
        {
            Debug.Log("Null tile at " + position + "   " + binaryType);
        }
    }
}
