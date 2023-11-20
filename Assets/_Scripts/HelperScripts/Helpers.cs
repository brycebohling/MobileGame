using System.Collections;
using System.Collections.Generic;
using Pathfinding.Ionic.Zip;
using UnityEngine;

public static class Helpers
{
    public enum Directions
    {
        Up,
        Down,
        Right,
        Left,
    }

    public static void ChangeAnimationState(Animator animator, string newState, float animSpeed)
    {
        animator.speed = animSpeed;
        animator?.Play(newState);
    }

    public static bool IsAnimationPlaying(Animator animator, string stateName)
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName(stateName) && animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
        {
            return true;
        } else
        {
            return false;
        }
    }

    public static bool CanPropBeAccessed(Room room, Vector2Int startPosition, List<Vector2Int> endPositions)
    {
        Queue<Vector2Int> pathQueue = new();
        HashSet<Vector2Int> visitedPaths = new() { startPosition };

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
                        return true;
                    }
                }

                if (!visitedPaths.Contains(edge) && room.FloorTiles.Contains(edge) &&
                    !room.PropPositions.Contains(edge))
                {                    
                    visitedPaths.Add(edge);
                    pathQueue.Enqueue(edge);
                }
            }
        }

        return false;
    }
}
