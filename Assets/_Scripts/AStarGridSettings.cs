using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine;

public class AStarGridSettings : MonoBehaviour
{
    [SerializeField] DungeonGenerator dungeonGeneratorScript;
    [SerializeField] LayerMask obstacleLayerMask;
    AstarData astarData;
    GridGraph gridScript;
    uint gridIndex;
    string gridName = "PCG Grid";
    bool hasSpawnedGraph;

    
    public void SetGridSize()
    {
        // This holds all graph data
        astarData = AstarPath.active.data;
        
        gridScript = astarData.AddGraph(typeof(GridGraph)) as GridGraph;
        GraphCollision gridCollisionsScript = new();
        
        gridCollisionsScript.use2D = true;
        gridCollisionsScript.diameter = 1f;
        gridCollisionsScript.mask = obstacleLayerMask;
        
        int width = dungeonGeneratorScript.dungeonWidth * 2;
        int depth = dungeonGeneratorScript.dungeonHeight * 2;
        float nodeSize = 0.5f;

        gridScript.SetGridShape(InspectorGridMode.Grid);
        gridScript.is2D = true;
        gridScript.SetDimensions(width, depth, nodeSize);
        gridScript.center = new Vector2(width, depth);
        gridScript.neighbours = NumNeighbours.Four;
        gridScript.erodeIterations = 0;
        gridScript.collision = gridCollisionsScript;

        gridIndex = gridScript.graphIndex;
        gridName = gridScript.name;
        hasSpawnedGraph = true;

        AstarPath.active.Scan();
    }

    public void DestroyGrid()
    {
        if (!hasSpawnedGraph) return;

        astarData = AstarPath.active.data;

        if (astarData.FindGraph(g => g.name == gridName) != null) 
        {
            Debug.Log("removed at " + gridIndex);
            astarData.RemoveGraph(astarData.graphs[gridIndex]);
        }
    }
}
