using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyParameters_", menuName = "Enemies/EnemySO")]
public class EnemySO : ScriptableObject
{
    [Header("Enemy Data")]
    public Transform EnemyPrefab;
    public Vector2Int EnemySize = Vector2Int.one;
    public Vector2 Center;
    public int EnemyDifficulty;

    [Header("Placement Type")]
    public bool MustBeAccessible;
    // [Min(1)] public int PlacementQuantityMin = 1;
    // [Min(1)] public int PlacementQuantityMax = 1;

    // [Header("Group Placement")]
    // public bool PlaceAsGroup;
    // [Min(1)] public int GroupMinCount = 1;
    // [Min(1)] public int GroupMaxCount = 1;
}
