using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractDungeonGenerator : MonoBehaviour
{
    [SerializeField] protected TilemapSpawner  tilemapSpawnerScript = null;

    [SerializeField] protected Vector2Int startPosition = Vector2Int.zero;

    public void GenerateDungeon()
    {
        tilemapSpawnerScript.Clear();
        RunProceduralGeneration();
    }

    protected abstract void RunProceduralGeneration();
}
