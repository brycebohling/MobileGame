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


    public void ProcessRooms()
    {
        if (dungeonGeneratorScript == null) return;
        
        foreach (Room room in dungeonGeneratorScript.RoomList)
        {
            PlaceEnemies(room, enemiesToPlace);
        }

        OnFinishedEnemyPlacement?.Invoke();
    }

    private void PlaceEnemies(Room room, List<EnemySO> enemyList)
    {
        int curDifficulty = 0;
        while (curDifficulty < room.RoomDifficulty)
        {
            int randomEnemyIndex = UnityEngine.Random.Range(0, enemyList.Count);

            HashSet<Vector2Int> accessiblePositions = new(room.FloorTiles);

            accessiblePositions.ExceptWith(dungeonGeneratorScript.Path);

            accessiblePositions.ExceptWith(room.PropPositions);
            accessiblePositions.ExceptWith(room.EnemyPositions);
            accessiblePositions.ExceptWith(room.ClearFloorTiles);

            List<Vector2Int> possiblePositions = accessiblePositions.OrderBy(x => Guid.NewGuid()).ToList();
                
            if (TryPlacingEnemyBruteForce(room, enemyList[randomEnemyIndex], possiblePositions)) 
            {
                curDifficulty += enemyList[randomEnemyIndex].EnemyDifficulty;
            }
        }
    }

    private bool TryPlacingEnemyBruteForce(Room room, EnemySO enemyToPlace, List<Vector2Int> possiblePositions)
    {
        for (int i = 0; i < possiblePositions.Count; i++)
        {
            Vector2Int position = possiblePositions[i];

            List<Vector2Int> freePositionsAround = TryToFitEnemy(enemyToPlace, possiblePositions, position);
            
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

    private List<Vector2Int> TryToFitEnemy(EnemySO enemyToPlace, List<Vector2Int> availablePositions, Vector2Int originPosition)
    {
        List<Vector2Int> freePositions = new();

        for (int xOffset = 0; xOffset < enemyToPlace.EnemySize.x; xOffset++)
        {
            for (int yOffset = 0; yOffset < enemyToPlace.EnemySize.y; yOffset++)
            {
                Vector2Int tempPos = originPosition + new Vector2Int(xOffset, yOffset);

                if (availablePositions.Contains(tempPos)) freePositions.Add(tempPos);
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