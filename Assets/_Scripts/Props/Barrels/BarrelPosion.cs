using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrelPosion : AbstractBarrel
{
    public override void SpawnBarrelInsides(Vector2 senderPos)
    {
        Debug.Log("Posion");
    } 
}
