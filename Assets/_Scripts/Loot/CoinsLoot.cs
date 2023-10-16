using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinsLoot : MonoBehaviour, ILoot
{
    public void Spawn()
    {
        Debug.Log("Spawned coins");
    }
}
