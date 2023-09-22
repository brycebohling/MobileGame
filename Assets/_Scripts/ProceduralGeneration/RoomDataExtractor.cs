using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RoomDataExtractor : MonoBehaviour
{
    [SerializeField] DungeonGenerator roomFirstMapGeneratorScript;
    public UnityEvent OnFinishRoomProcessing;


    public void ProcessRooms()
    {
        if (roomFirstMapGeneratorScript == null) return;
        
        foreach (Room room in roomFirstMapGeneratorScript.RoomList)
        {
            foreach (Vector2Int tilePosition in room.FloorTiles)
            {
                int neighbourCount = 0;

                if (!room.FloorTiles.Contains(tilePosition + Vector2Int.up))
                {
                    room.NearWallTilesUp.Add(tilePosition);
                    neighbourCount++;
                }

                if (!room.FloorTiles.Contains(tilePosition + Vector2Int.down))
                {
                    room.NearWallTilesDown.Add(tilePosition);
                    neighbourCount++;
                }

                if (!room.FloorTiles.Contains(tilePosition + Vector2Int.right))
                {
                    room.NearWallTilesRight.Add(tilePosition);
                    neighbourCount++;
                }

                if (!room.FloorTiles.Contains(tilePosition + Vector2Int.left))
                {
                    room.NearWallTilesLeft.Add(tilePosition);
                    neighbourCount++;
                }

                if (neighbourCount >= 2)
                {
                    room.CornerTiles.Add(tilePosition);

                } else if(neighbourCount == 0)
                {
                    room.InnerTiles.Add(tilePosition);
                }
            }

            room.NearWallTilesUp.ExceptWith(room.CornerTiles);
            room.NearWallTilesDown.ExceptWith(room.CornerTiles);
            room.NearWallTilesRight.ExceptWith(room.CornerTiles);
            room.NearWallTilesLeft.ExceptWith(room.CornerTiles);
        }

        OnFinishRoomProcessing?.Invoke();
    
    }
}
