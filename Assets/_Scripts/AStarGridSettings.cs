using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine;
using UnityEngine.Events;

public class AStarGridSettings : MonoBehaviour
{
    public UnityEvent OnFinishedGridScan;

    [SerializeField] DungeonGenerator dungeonGeneratorScript;

    [Header("Grid Settings")]
    [SerializeField] float nodeSize;
    [SerializeField] int erode;
    [SerializeField] LayerMask obstacleLayerMask;

    AstarData astarData;
    GridGraph gridScript;
    uint gridIndex;
    string gridName = "PCG Grid";
    bool hasSpawnedGraph;
    

    public void ScanGraphs()
    {
        AstarData.active.Scan();

        OnFinishedGridScan?.Invoke();
    }

    public void InitializeGrid()
    {
        // This holds all graph data
        astarData = AstarPath.active.data;
        
        gridScript = astarData.AddGraph(typeof(GridGraph)) as GridGraph;
        GraphCollision gridCollisionsScript = new()
        {
            use2D = true,
            diameter = 1f,
            mask = obstacleLayerMask
        };

        int width = dungeonGeneratorScript.dungeonWidth * Mathf.RoundToInt(Mathf.Pow(nodeSize, -1));
        int depth = dungeonGeneratorScript.dungeonHeight * Mathf.RoundToInt(Mathf.Pow(nodeSize, -1));

        gridScript.SetGridShape(InspectorGridMode.Grid);
        gridScript.is2D = true;
        gridScript.SetDimensions(width, depth, nodeSize);
        gridScript.center = new Vector2(width / (Mathf.RoundToInt(Mathf.Pow(nodeSize, -1)) * 2), depth / (Mathf.RoundToInt(Mathf.Pow(nodeSize, -1)) * 2));
        gridScript.neighbours = NumNeighbours.Four;
        gridScript.erodeIterations = erode;
        gridScript.collision = gridCollisionsScript;

        gridIndex = gridScript.graphIndex;
        gridName = gridScript.name;
        hasSpawnedGraph = true;

        Invoke("ScanGraphs", 0.01f);
    }

    public void DestroyGrid()
    {
        if (!hasSpawnedGraph) return;

        if (astarData.FindGraph(g => g.name == gridName) != null) 
        {
            Debug.Log("removed at " + gridIndex);
            astarData.RemoveGraph(astarData.graphs[gridIndex]);
        }
    }
}
