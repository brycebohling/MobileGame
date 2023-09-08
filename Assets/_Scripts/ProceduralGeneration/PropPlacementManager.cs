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
    [SerializeField, Range(0, 1)] float cornerPropPlacementChance;

    [SerializeField] GameObject propPrefab;

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
            List<PropSO> cornerProps = propsToPlace.Where(x => x.Corner).ToList();
            PlaceCornerProps(room, cornerProps);

            List<PropSO> leftWallProps = propsToPlace.Where(x => x.NearWallLeft)
                .OrderByDescending(x => x.PropSize.x * x.PropSize.y).ToList();
            PlaceProps(room, leftWallProps, room.NearWallTilesLeft, PlacementOriginCorner.BottomLeft);

            List<PropSO> rightWallProps = propsToPlace.Where(x => x.NearWallLeft)
                .OrderByDescending(x => x.PropSize.x * x.PropSize.y).ToList();
            PlaceProps(room, rightWallProps, room.NearWallTilesRight, PlacementOriginCorner.TopRight);

            List<PropSO> upWallProps = propsToPlace.Where(x => x.NearWallUp)
                .OrderByDescending(x => x.PropSize.x * x.PropSize.y).ToList();
            PlaceProps(room, upWallProps, room.NearWallTilesUp, PlacementOriginCorner.TopLeft);

            List<PropSO> downWallProps = propsToPlace.Where(x => x.NearWallDown)
                .OrderByDescending(x => x.PropSize.x * x.PropSize.y).ToList();
            PlaceProps(room, downWallProps, room.NearWallTilesDown, PlacementOriginCorner.TopRight);

            List<PropSO> innerProps = propsToPlace.Where(x => x.Inner)
                .OrderByDescending(x => x.PropSize.x * x.PropSize.y).ToList();
            PlaceProps(room, innerProps, room.InnerTiles, PlacementOriginCorner.BottomLeft);
        }

        OnFinishedPropPlacement?.Invoke();
    }

    private void PlaceProps(Room room, List<PropSO> wallProps, HashSet<Vector2Int> availableTiles, PlacementOriginCorner placement)
    {
        HashSet<Vector2Int> tempPositions = new(availableTiles);
        tempPositions.ExceptWith(roomFirstMapGeneratorScript.Path);

        foreach (PropSO propToPlace in wallProps)
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
                PlacePropGameObjectAt(room, position, propToPlace);

                foreach (Vector2Int pos in freePositionsAround)
                {
                    room.PropPositions.Add(pos);
                }

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

    private GameObject PlacePropGameObjectAt(Room room, Vector2Int placementPosition, PropSO propToPlace)
    {
        GameObject prop = Instantiate(propPrefab);
        SpriteRenderer propSpriteRenderer = prop.GetComponentInChildren<SpriteRenderer>();

        propSpriteRenderer.sprite = propToPlace.PropSprite;

        CapsuleCollider2D collider = propSpriteRenderer.gameObject.AddComponent<CapsuleCollider2D>();
        collider.offset = Vector2.zero;

        if (propToPlace.PropSize.x > propToPlace.PropSize.y)
        {
            collider.direction = CapsuleDirection2D.Horizontal;
        }
        
        float colliderPercent = 0.8f;

        Vector2 size = new (propToPlace.PropSize.x * colliderPercent, propToPlace.PropSize.y * colliderPercent);
        collider.size = size;

        prop.transform.localPosition = (Vector2)placementPosition;

        propSpriteRenderer.transform.localPosition = (Vector2)propToPlace.PropSize * 0.5f;

        room.PropPositions.Add(placementPosition);
        room.PropObjectReference.Add(prop);
        return prop;
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

    private void PlaceCornerProps(Room room, List<PropSO> cornerProps)
    {
        float tempChance = cornerPropPlacementChance;

        foreach (Vector2Int cornerTile in room.CornerTiles)
        {
            if (UnityEngine.Random.value < tempChance)
            {
                PropSO propToPlace = cornerProps[UnityEngine.Random.Range(0, cornerProps.Count)];

                PlacePropGameObjectAt(room, cornerTile, propToPlace);

                if (propToPlace.PlaceAsGroup)
                {
                    PlaceGroupObjects(room, cornerTile, propToPlace, 2);
                }
            }
        }
    }
}

public enum PlacementOriginCorner
{
    TopRight,
    TopLeft,
    BottomRight,
    BottomLeft,
}
