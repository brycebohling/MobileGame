using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Linq;
using System;

public class EnemyPlacementManager : MonoBehaviour
{
    public UnityEvent OnFinishedEnemyPlacement;

    [SerializeField] DungeonGenerator dungeonGeneratorScript;
    [SerializeField] List<EnemySO> enemiesToPlace;
    [SerializeField] Transform enemyParentPrefab;


    public void ProcessToRooms()
    {
        if (dungeonGeneratorScript == null) return;
        
        foreach (Room room in dungeonGeneratorScript.RoomList)
        {
            PlaceProps(room, enemiesToPlace, PlacementOriginCorner.BottomLeft);
        }

        OnFinishedEnemyPlacement?.Invoke();
    }

    private void PlaceProps(Room room, List<EnemySO> enemyList, PlacementOriginCorner placement)
    {
        foreach (EnemySO enemyToPlace in enemyList)
        {
            HashSet<Vector2Int> accessiblePositions = new(room.FloorTiles);

            accessiblePositions.ExceptWith(dungeonGeneratorScript.Path);

            int quantity = UnityEngine.Random.Range(enemyToPlace.PlacementQuantityMin, enemyToPlace.PlacementQuantityMax + 1);

            for (int i = 0; i < quantity; i++)
            {
                accessiblePositions.ExceptWith(room.PropPositions);
                accessiblePositions.ExceptWith(room.EnemyPositions);
                accessiblePositions.ExceptWith(room.ClearFloorTiles);

                List<Vector2Int> possiblePositions = accessiblePositions.OrderBy(x => Guid.NewGuid()).ToList();
                
                if (!TryPlacingEnemyBruteForce(room, enemyToPlace, possiblePositions, placement)) break;
            }
        }
    }

    private bool TryPlacingEnemyBruteForce(Room room, EnemySO enemyToPlace, List<Vector2Int> possiblePositions, PlacementOriginCorner placement)
    {
        for (int i = 0; i < possiblePositions.Count; i++)
        {
            Vector2Int position = possiblePositions[i];

            List<Vector2Int> freePositionsAround = TryToFitProp(enemyToPlace, possiblePositions, position, placement);
            
            if (freePositionsAround.Count == enemyToPlace.EnemySize.x * enemyToPlace.EnemySize.y)
            {
                if (enemyToPlace.MustBeAccessible)
                {
                    if (!Helpers.CanPropBeAccessed(room, room.RoomCenterPos, freePositionsAround)) continue;
                }
                    
                PlaceEnemyGameObjectAt(room, position, enemyToPlace);

                // if (enemyToPlace.PlaceAsGroup)
                // {
                //     PlaceGroupObjects(room, position, enemyToPlace, 1);
                // }

                return true;
            }
        }

        return false;
    }

    private List<Vector2Int> TryToFitProp(EnemySO enemyToPlace, List<Vector2Int> availablePositions,
        Vector2Int originPosition, PlacementOriginCorner placement)
    {
        List<Vector2Int> freePositions = new();

        if (placement == PlacementOriginCorner.BottomLeft)
        {
            for (int xOffset = 0; xOffset < enemyToPlace.EnemySize.x; xOffset++)
            {
                for (int yOffset = 0; yOffset < enemyToPlace.EnemySize.y; yOffset++)
                {
                    Vector2Int tempPos = originPosition + new Vector2Int(xOffset, yOffset);

                    if (availablePositions.Contains(tempPos)) freePositions.Add(tempPos);
                }
            }

        } else if (placement == PlacementOriginCorner.BottomRight)
        {
            for (int xOffset = -enemyToPlace.EnemySize.x + 1; xOffset <= 0; xOffset++)
            {
                for (int yOffset = 0; yOffset < enemyToPlace.EnemySize.y; yOffset++)
                {
                    Vector2Int tempPos = originPosition + new Vector2Int(xOffset, yOffset);

                    if (availablePositions.Contains(tempPos)) freePositions.Add(tempPos);
                }
            }

        } else if (placement == PlacementOriginCorner.TopLeft)
        {
            for (int xOffset = 0; xOffset < enemyToPlace.EnemySize.x; xOffset++)
            {
                for (int yOffset = -enemyToPlace.EnemySize.y + 1; yOffset <= 0; yOffset++)
                {
                    Vector2Int tempPos = originPosition + new Vector2Int(xOffset, yOffset);

                    if (availablePositions.Contains(tempPos)) freePositions.Add(tempPos);
                }
            }

        } else
        {
            for (int xOffset = -enemyToPlace.EnemySize.x + 1; xOffset <= 0; xOffset++)
            {
                for (int yOffset = -enemyToPlace.EnemySize.y + 1; yOffset <= 0; yOffset++)
                {
                    Vector2Int tempPos = originPosition + new Vector2Int(xOffset, yOffset);

                    if (availablePositions.Contains(tempPos)) freePositions.Add(tempPos);
                }
            }
        }

        return freePositions;
    }

    private void PlaceEnemyGameObjectAt(Room room, Vector2Int placementPosition, EnemySO enemyToPlace)
    {
        Transform enemyParent = Instantiate(enemyParentPrefab);
        Transform enemy = Instantiate(enemyToPlace.EnemyPrefab, Vector2.zero, Quaternion.identity, enemyParent.transform);

        enemyParent.localPosition = (Vector2)placementPosition;
        enemy.localPosition = enemyToPlace.Center;

        room.EnemyPositions.Add(placementPosition);
        room.EnemyTransfromReference.Add(enemyParent);

        if (enemyToPlace.MustBeAccessible)
        {
            bool isAccessible = Helpers.CanPropBeAccessed(room, new Vector2Int(Mathf.RoundToInt(room.RoomCenterPos.x), Mathf.RoundToInt(room.RoomCenterPos.y)),
                new List<Vector2Int>() {placementPosition});

            if (!isAccessible)
            {
                Debug.Log("not found (Accessible): " + placementPosition);
            }

        }
    }
}