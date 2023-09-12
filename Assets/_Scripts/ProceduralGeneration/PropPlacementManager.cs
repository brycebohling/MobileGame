using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class PropPlacementManager : MonoBehaviour
{
    RoomFirstMapGenerator roomFirstMapGeneratorScript;

    [SerializeField] List<PropSO> propsToPlace;
    [SerializeField] Transform propParentPrefab;

    public UnityEvent OnFinishedPropPlacement;

    private void Awake() 
    {
        roomFirstMapGeneratorScript = FindObjectOfType<RoomFirstMapGenerator>();
    }

    public void ProcessToRooms()
    {
        if (roomFirstMapGeneratorScript == null) return;
        
        foreach (Room room in roomFirstMapGeneratorScript.RoomList)
        {
            // List<PropSO> accessibleProps = propsToPlace.Where(x => x.mustBeAccessible).ToList()
            //     .OrderByDescending(x => x.PropSize.x * x.PropSize.y).ToList();
            // PlaceAccessibleProps(room, accessibleProps);

            // List<PropSO> cornerProps = propsToPlace.Where(x => x.Corner && !x.mustBeAccessible).ToList()
            //     .OrderByDescending(x => x.PropSize.x * x.PropSize.y).ToList();
 
            // if (cornerProps.Count != 0) PlaceProps(room, cornerProps, room.CornerTiles, PlacementOriginCorner.BottomLeft);

            List<PropSO> leftWallProps = propsToPlace.Where(x => x.NearWallLeft && !x.mustBeAccessible)
                .OrderByDescending(x => x.PropSize.x * x.PropSize.y).ToList();

            if (leftWallProps.Count != 0) PlaceProps(room, leftWallProps, room.NearWallTilesLeft, PlacementOriginCorner.BottomLeft);

            List<PropSO> rightWallProps = propsToPlace.Where(x => x.NearWallLeft && !x.mustBeAccessible)
                .OrderByDescending(x => x.PropSize.x * x.PropSize.y).ToList();

            if (rightWallProps.Count != 0) PlaceProps(room, rightWallProps, room.NearWallTilesRight, PlacementOriginCorner.BottomLeft);

            List<PropSO> upWallProps = propsToPlace.Where(x => x.NearWallUp && !x.mustBeAccessible)
                .OrderByDescending(x => x.PropSize.x * x.PropSize.y).ToList();

            if (upWallProps.Count != 0) PlaceProps(room, upWallProps, room.NearWallTilesUp, PlacementOriginCorner.BottomLeft);

            List<PropSO> downWallProps = propsToPlace.Where(x => x.NearWallDown && !x.mustBeAccessible)
                .OrderByDescending(x => x.PropSize.x * x.PropSize.y).ToList();

            if (downWallProps.Count != 0) PlaceProps(room, downWallProps, room.NearWallTilesDown, PlacementOriginCorner.BottomLeft);

            List<PropSO> innerProps = propsToPlace.Where(x => x.Inner && !x.mustBeAccessible)
                .OrderByDescending(x => x.PropSize.x * x.PropSize.y).ToList();

            if (innerProps.Count != 0) PlaceProps(room, innerProps, room.InnerTiles, PlacementOriginCorner.BottomLeft);

            List<PropSO> cornerProps = propsToPlace.Where(x => x.Corner && x.mustBeAccessible).ToList()
                .OrderByDescending(x => x.PropSize.x * x.PropSize.y).ToList();
 
            if (cornerProps.Count != 0) PlaceProps(room, cornerProps, room.CornerTiles, PlacementOriginCorner.BottomLeft);
        }

        OnFinishedPropPlacement?.Invoke();
    }

    private void PlaceAccessibleProps(Room room, List<PropSO> accessibleProps)
    {
        // pick a loaction to put a prop
        // check to make sure no path abstruction
        // place accessible prop

        // place normal props to not abstruct the path

        List<PropSO> cornerProps = propsToPlace.Where(x => x.Corner).ToList()
            .OrderByDescending(x => x.PropSize.x * x.PropSize.y).ToList();

        if (cornerProps.Count != 0) PlaceProps(room, cornerProps, room.CornerTiles, PlacementOriginCorner.BottomLeft);

        List<PropSO> leftWallProps = propsToPlace.Where(x => x.NearWallLeft)
            .OrderByDescending(x => x.PropSize.x * x.PropSize.y).ToList();

        if (leftWallProps.Count != 0) PlaceProps(room, leftWallProps, room.NearWallTilesLeft, PlacementOriginCorner.BottomLeft);

        List<PropSO> rightWallProps = propsToPlace.Where(x => x.NearWallLeft)
            .OrderByDescending(x => x.PropSize.x * x.PropSize.y).ToList();

        if (rightWallProps.Count != 0) PlaceProps(room, rightWallProps, room.NearWallTilesRight, PlacementOriginCorner.BottomLeft);

        List<PropSO> upWallProps = propsToPlace.Where(x => x.NearWallUp)
            .OrderByDescending(x => x.PropSize.x * x.PropSize.y).ToList();

        if (upWallProps.Count != 0) PlaceProps(room, upWallProps, room.NearWallTilesUp, PlacementOriginCorner.BottomLeft);

        List<PropSO> downWallProps = propsToPlace.Where(x => x.NearWallDown)
            .OrderByDescending(x => x.PropSize.x * x.PropSize.y).ToList();

        if (downWallProps.Count != 0) PlaceProps(room, downWallProps, room.NearWallTilesDown, PlacementOriginCorner.BottomLeft);

        List<PropSO> innerProps = propsToPlace.Where(x => x.Inner)
            .OrderByDescending(x => x.PropSize.x * x.PropSize.y).ToList();

        if (innerProps.Count != 0) PlaceProps(room, innerProps, room.InnerTiles, PlacementOriginCorner.BottomLeft);
    }

    private void PlaceProps(Room room, List<PropSO> propsList, HashSet<Vector2Int> availableTiles, PlacementOriginCorner placement)
    {
        HashSet<Vector2Int> tempPositions = new(availableTiles);
        tempPositions.ExceptWith(roomFirstMapGeneratorScript.Path);

        foreach (PropSO propToPlace in propsList)
        {
            int quantity = UnityEngine.Random.Range(propToPlace.PlacementQuantityMin, propToPlace.PlacementQuantityMax + 1);

            for (int i = 0; i < quantity; i++)
            {
                tempPositions.ExceptWith(room.PropPositions);
                List<Vector2Int> availablePositions = tempPositions.OrderBy(x => Guid.NewGuid()).ToList();

                if (!TryPlacingPropBruteForce(room, propToPlace, availablePositions, placement)) break;
            }
        }
    }

    private bool TryPlacingPropBruteForce(Room room, PropSO propToPlace, List<Vector2Int> availablePositions, PlacementOriginCorner placement)
    {
        for (int i = 0; i < availablePositions.Count; i++)
        {
            Vector2Int position = availablePositions[i];

            if (room.PropPositions.Contains(position)) continue;

            List<Vector2Int> freePositionsAround = TryToFitProp(propToPlace, availablePositions, position, placement);

            if (freePositionsAround.Count == propToPlace.PropSize.x * propToPlace.PropSize.y)
            {
                // if (!CanBeAccessed(room, room.RoomCenterPos, freePositionsAround)) continue;

                PlacePropGameObjectAt(room, position, propToPlace);

                if (propToPlace.PlaceAsGroup)
                {
                    PlaceGroupObjects(room, position, propToPlace, 1);
                }

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

        prop.localPosition = (Vector2)propToPlace.PropSize * 0.5f;

        room.PropPositions.Add(placementPosition);
        room.PropObjectReference.Add(propParent.gameObject);

        if (propToPlace.mustBeAccessible)
        {
            room.AccessiblePropPositions.Add(placementPosition);

            CanBeAccessed(room, new Vector2Int(Mathf.RoundToInt(room.RoomCenterPos.x), Mathf.RoundToInt(room.RoomCenterPos.y)),
                new List<Vector2Int>() {placementPosition});
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
                
                if (room.FloorTiles.Contains(tempPos) && !roomFirstMapGeneratorScript.Path.Contains(tempPos) &&
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

    private bool CanBeAccessed(Room room, Vector2Int startPos, List<Vector2Int> endPositions)
    {
        Queue<Vector2Int> pathQueue = new();
        HashSet<Vector2Int> visitedPaths = new() { startPos };

        pathQueue.Enqueue(startPos);

        bool isAccessible = false;
        while (pathQueue.Count > 0) 
        {
            Vector2Int currentTile = pathQueue.Dequeue();

            foreach (var direction in Direction2D.cardinalDirectionsList)
            {
                Vector2Int edge = currentTile + direction;

                if (!visitedPaths.Contains(edge) && room.FloorTiles.Contains(edge) &&
                    (room.AccessiblePropPositions.Contains(edge) || !room.PropPositions.Contains(edge)))
                {                    
                    visitedPaths.Add(edge);

                    // foreach (var position in endPositions)
                    // {
                    //     if (edge == position)
                    //     {
                    //         isAccessible = true;
                    //         break;
                    //     } 

                    //     pathQueue.Enqueue(edge);
                    // }

                    if (edge == endPositions[0])
                    {
                        isAccessible = true;
                        break;
                    } 

                    pathQueue.Enqueue(edge);
                }
            }
        }

        if (isAccessible)
        {
            Debug.Log("found");
            return true;
        }  
        
        Debug.Log("not found");
        return false;
    }
}

public enum PlacementOriginCorner
{
    TopRight,
    TopLeft,
    BottomRight,
    BottomLeft,
}
