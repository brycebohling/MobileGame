using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class PropSO : ScriptableObject
{
    [Header("Prop Data")]
    public Sprite PropSprite;
    public Vector2Int PropSize = Vector2Int.one;

    [Space, Header("Placement Type")]
    public bool Corner;
    public bool Inner;
    public bool NearWallUp;
    public bool NearWallDown;
    public bool NearWallRight;
    public bool NearWallLeft;
    [Min(1)] public int PlacementQuantityMin = 1;
    [Min(1)] public int PlacementQuantityMax = 1;

    [Space, Header("Group Placement")]
    public bool PlaceAsGroup;
    [Min(1)] public int GroupMinCount = 1;
    [Min(1)] public int GroupMaxCount = 1;
}
