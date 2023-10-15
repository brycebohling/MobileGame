using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class PropPlacementManager : MonoBehaviour
{
    public UnityEvent OnFinishedPropPlacement;

    [SerializeField] DungeonGenerator dungeonGeneratorScript;
    [SerializeField] List<PropSO> propsToPlace;
    [SerializeField] Transform propParentPrefab;


    public void ProcessRooms()
    {
        if (dungeonGeneratorScript == null) return;
        
        foreach (Room room in dungeonGeneratorScript.RoomList)
        {
            PlaceMustHaveProps(room);

            PlaceAccessibleProps(room);
            
            PlaceNormalProps(room);   
        }

        OnFinishedPropPlacement?.Invoke();
    }

    private void PlaceMustHaveProps(Room room)
    {
        List<PropSO> mustHaveProps = propsToPlace.Where(x => x.mustBePlacedAndAccessible)
            .OrderByDescending(x => x.PropSize.x * x.PropSize.y).ToList();

        if (mustHaveProps.Count != 0) PlaceProps(room, mustHaveProps, PlacementOriginCorner.BottomLeft);
    }

    private void PlaceAccessibleProps(Room room)
    {
        List<PropSO> accessibleProps = propsToPlace.Where(x => x.mustBeAccessible)
            .OrderByDescending(x => x.PropSize.x * x.PropSize.y).ToList();

        if (accessibleProps.Count != 0) PlaceProps(room, accessibleProps, PlacementOriginCorner.BottomLeft);
    }

    private void PlaceNormalProps(Room room)
    {
        List<PropSO> normalProps = propsToPlace.Where(x => !x.mustBeAccessible && !x.mustBePlacedAndAccessible)
            .OrderByDescending(x => x.PropSize.x * x.PropSize.y).ToList();

        if (normalProps.Count != 0) PlaceProps(room, normalProps, PlacementOriginCorner.BottomLeft);
    }

    private void PlaceProps(Room room, List<PropSO> propsList, PlacementOriginCorner placement)
    {
        foreach (PropSO propToPlace in propsList)
        {
            HashSet<Vector2Int> accessiblePositions = new();

            if (propToPlace.Corner)
            {
                accessiblePositions.UnionWith(room.CornerTiles);
            }

            if (propToPlace.NearWallLeft)
            {
                accessiblePositions.UnionWith(room.NearWallTilesLeft);
            }

            if (propToPlace.NearWallRight)
            {
                accessiblePositions.UnionWith(room.NearWallTilesRight);
            }

            if (propToPlace.NearWallUp)
            {
                accessiblePositions.UnionWith(room.NearWallTilesUp);
            }

            if (propToPlace.NearWallDown)
            {
                accessiblePositions.UnionWith(room.NearWallTilesDown);
            }

            if (propToPlace.Inner)
            {
                accessiblePositions.UnionWith(room.InnerTiles);
            }

            accessiblePositions.ExceptWith(dungeonGeneratorScript.Path);

            int quantity = UnityEngine.Random.Range(propToPlace.PlacementQuantityMin, propToPlace.PlacementQuantityMax + 1);

            for (int i = 0; i < quantity; i++)
            {
                accessiblePositions.ExceptWith(room.PropPositions);
                accessiblePositions.ExceptWith(room.ClearFloorTiles);

                List<Vector2Int> possiblePositions = accessiblePositions.OrderBy(x => Guid.NewGuid()).ToList();

                if (!TryPlacingPropBruteForce(room, propToPlace, possiblePositions, placement)) break;
            }
        }
    }

    private bool TryPlacingPropBruteForce(Room room, PropSO propToPlace, List<Vector2Int> possiblePositions, PlacementOriginCorner placement)
    {
        for (int i = 0; i < possiblePositions.Count; i++)
        {
            Vector2Int position = possiblePositions[i];

            List<Vector2Int> freePositionsAround = TryToFitProp(propToPlace, possiblePositions, position, placement);

            if (freePositionsAround.Count == propToPlace.PropSize.x * propToPlace.PropSize.y)
            {
                // If on last iteration
                if (possiblePositions.Count == i + 1 && propToPlace.mustBePlacedAndAccessible)
                {
                    if (!Helpers.CanPropBeAccessed(room, room.RoomCenterPos, freePositionsAround))
                    {
                        ClearAPathToProp(room, room.RoomCenterPos, freePositionsAround);
                    }
                
                } else
                {
                    if (propToPlace.mustBeAccessible)
                    {
                        if (!Helpers.CanPropBeAccessed(room, room.RoomCenterPos, freePositionsAround)) continue;
                    }
                }

                PlacePropGameObjectAt(room, position, propToPlace);

                // if (propToPlace.PlaceAsGroup)
                // {
                //     PlaceGroupObjects(room, position, propToPlace, 1);
                // }

                return true;
            }
        }

        return false;
    }

    private List<Vector2Int> TryToFitProp(PropSO propToPlace, List<Vector2Int> availablePositions,
        Vector2Int originPosition, PlacementOriginCorner placement)
    {
        List<Vector2Int> freePositions = new();

        if (placement == PlacementOriginCorner.BottomLeft)
        {
            for (int xOffset = 0; xOffset < propToPlace.PropSize.x; xOffset++)
            {
                for (int yOffset = 0; yOffset < propToPlace.PropSize.y; yOffset++)
                {
                    Vector2Int tempPos = originPosition + new Vector2Int(xOffset, yOffset);

                    if (availablePositions.Contains(tempPos)) freePositions.Add(tempPos);
                }
            }

        } else if (placement == PlacementOriginCorner.BottomRight)
        {
            for (int xOffset = -propToPlace.PropSize.x + 1; xOffset <= 0; xOffset++)
            {
                for (int yOffset = 0; yOffset < propToPlace.PropSize.y; yOffset++)
                {
                    Vector2Int tempPos = originPosition + new Vector2Int(xOffset, yOffset);

                    if (availablePositions.Contains(tempPos)) freePositions.Add(tempPos);
                }
            }

        } else if (placement == PlacementOriginCorner.TopLeft)
        {
            for (int xOffset = 0; xOffset < propToPlace.PropSize.x; xOffset++)
            {
                for (int yOffset = -propToPlace.PropSize.y + 1; yOffset <= 0; yOffset++)
                {
                    Vector2Int tempPos = originPosition + new Vector2Int(xOffset, yOffset);

                    if (availablePositions.Contains(tempPos)) freePositions.Add(tempPos);
                }
            }

        } else
        {
            for (int xOffset = -propToPlace.PropSize.x + 1; xOffset <= 0; xOffset++)
            {
                for (int yOffset = -propToPlace.PropSize.y + 1; yOffset <= 0; yOffset++)
                {
                    Vector2Int tempPos = originPosition + new Vector2Int(xOffset, yOffset);

                    if (availablePositions.Contains(tempPos)) freePositions.Add(tempPos);
                }
            }
        }

        return freePositions;
    }

    private void PlacePropGameObjectAt(Room room, Vector2Int placementPosition, PropSO propToPlace)
    {
        Transform propParent = Instantiate(propParentPrefab);
        Transform prop = Instantiate(propToPlace.PropPrefab, Vector2.zero, Quaternion.identity, propParent.transform);

        propParent.localPosition = (Vector2)placementPosition;

        room.PropPositions.Add(placementPosition);
        room.PropTransfromReference.Add(propParent);

        if (propToPlace.mustBeAccessible)
        {
            bool isAccessible = Helpers.CanPropBeAccessed(room, new Vector2Int(Mathf.RoundToInt(room.RoomCenterPos.x), Mathf.RoundToInt(room.RoomCenterPos.y)),
                new List<Vector2Int>() {placementPosition});

            if (!isAccessible)
            {
                Debug.Log("not found (Accessible)");
            }

        } else if (propToPlace.mustBePlacedAndAccessible)
        {
            ClearAPathToProp(room, room.RoomCenterPos, new List<Vector2Int>() {placementPosition});


            bool isAccessible = Helpers.CanPropBeAccessed(room, new Vector2Int(Mathf.RoundToInt(room.RoomCenterPos.x), Mathf.RoundToInt(room.RoomCenterPos.y)),
                new List<Vector2Int>() {placementPosition});

            if (!isAccessible)
            {
                Debug.Log("not found (Accessible&Placed)");
            }
        }

        if (propToPlace.fourSpriteDirections)
        {
            if (!room.FloorTiles.Contains(placementPosition + Vector2Int.up))
            {
                prop.GetComponent<SpriteRenderer>().sprite = propToPlace.spriteFront;

            } else if (!room.FloorTiles.Contains(placementPosition + Vector2Int.down))
            {
                prop.GetComponent<SpriteRenderer>().sprite = propToPlace.spriteBack;

            } else if (!room.FloorTiles.Contains(placementPosition + Vector2Int.right))
            {
                prop.GetComponent<SpriteRenderer>().sprite = propToPlace.spriteLeft;

            } else if (!room.FloorTiles.Contains(placementPosition + Vector2Int.left))
            {
                prop.GetComponent<SpriteRenderer>().sprite = propToPlace.spriteRight;

            }
        }

        if (propToPlace.hasVariants)
        {
            Sprite variant = propToPlace.variants[UnityEngine.Random.Range(0, propToPlace.variants.Length)];

            prop.GetComponent<SpriteRenderer>().sprite = variant;
        }
    }

    private void PlaceGroupObjects(Room room, Vector2Int groupOriginPosition, PropSO propToPlace, int searchOffset)
    {
        int count = UnityEngine.Random.Range(propToPlace.GroupMinCount, propToPlace.GroupMinCount) - 1;
        count = Mathf.Clamp(count, 0, 8);

        List<Vector2Int> availableSpaces = new();

        for (int xOffset = -searchOffset; xOffset <= searchOffset; xOffset++)
        {
            for (int yOffset = -searchOffset; yOffset <= searchOffset; yOffset++)
            {
                Vector2Int tempPos = groupOriginPosition + new Vector2Int(xOffset, yOffset);
                
                if (room.FloorTiles.Contains(tempPos) && !dungeonGeneratorScript.Path.Contains(tempPos) &&
                    !room.PropPositions.Contains(tempPos))
                {
                    availableSpaces.Add(tempPos);
                }
            }
        }

        availableSpaces.OrderBy(x => Guid.NewGuid());

        int tempCount = count < availableSpaces.Count ? count : availableSpaces.Count;

        for (int i = 0; i < tempCount; i++)
        {
            PlacePropGameObjectAt(room, availableSpaces[i], propToPlace);
        }
    }

    private void ClearAPathToProp(Room room, Vector2Int startPosition, List<Vector2Int> endPositions)
    {
        Queue<Vector2Int> pathQueue = new();
        HashSet<Vector2Int> visitedPaths = new() { startPosition };
        Dictionary<Vector2Int, Vector2Int> pathParents = new();

        pathQueue.Enqueue(startPosition);

        while (pathQueue.Count > 0) 
        {
            Vector2Int currentTile = pathQueue.Dequeue();

            foreach (var direction in Direction2D.cardinalDirectionsList)
            {
                Vector2Int edge = currentTile + direction;

                foreach (var position in endPositions)
                {
                    if (edge == position)
                    {   
                        pathParents.Add(edge, currentTile);

                        Vector2Int temp = position;
                        List<Vector2Int> path = new() { temp };

                        while (temp != startPosition)
                        {
                            temp = pathParents[temp];
                            path.Add(temp);
                        }

                        foreach (var node in path)
                        {
                            room.ClearFloorTiles.Add(node);
                        }
                        
                        return;
                    }
                }

                if (!visitedPaths.Contains(edge) && room.FloorTiles.Contains(edge) &&
                    !room.PropPositions.Contains(edge))
                {                    
                    visitedPaths.Add(edge);
                    pathQueue.Enqueue(edge);

                    pathParents.Add(edge, currentTile);

                }
            }
        }

        return;
    }
}

public enum PlacementOriginCorner
{
    TopRight,
    TopLeft,
    BottomRight,
    BottomLeft,
}
