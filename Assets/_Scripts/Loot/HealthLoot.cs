using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthLoot : MonoBehaviour, ILoot
{
    public void Spawn()
    {
        Debug.Log("Spawned health");
    }
}
