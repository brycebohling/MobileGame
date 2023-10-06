using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine;

public class AStarGridSettings : MonoBehaviour
{
    [SerializeField] DungeonGenerator dungeonGeneratorScript;
    [SerializeField] LayerMask obstacleLayerMask;
    
    public void SetGridSize()
    {
        // This holds all graph data
        AstarData data = AstarPath.active.data;

        GridGraph gridGraphScript = data.AddGraph(typeof(GridGraph)) as GridGraph;
        GraphCollision gridCollisionsScript = new();
        
        gridCollisionsScript.use2D = true;
        gridCollisionsScript.diameter = 1f;
        gridCollisionsScript.mask = obstacleLayerMask;
        

        int width = dungeonGeneratorScript.dungeonWidth;
        int depth = dungeonGeneratorScript.dungeonHeight;
        float nodeSize = 0.5f;

        gridGraphScript.SetGridShape(InspectorGridMode.Grid);
        gridGraphScript.is2D = true;
        gridGraphScript.SetDimensions(width, depth, nodeSize);
        gridGraphScript.center = new Vector2(width / 2, depth / 2);
        gridGraphScript.neighbours = NumNeighbours.Four;
        gridGraphScript.erodeIterations = 0;
        gridGraphScript.collision = gridCollisionsScript;
        
        AstarPath.active.Scan();
    }
}
