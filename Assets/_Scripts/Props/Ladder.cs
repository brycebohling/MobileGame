using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Ladder : MonoBehaviour, IInteractable
{
    public void OnInteract()
    {
        GameObject.Find("DungeonGeneratorManager").GetComponent<DungeonGenerator>().GenerateNextLevel();
    }
}
