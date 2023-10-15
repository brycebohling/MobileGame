using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrelPosion : AbstractBarrel
{
    [SerializeField] ParticleSystem brakeParticles;

    public override void SpawnBarrelInsides(Vector2 senderPos)
    {
        Instantiate(brakeParticles, senderPos, Quaternion.identity);
    }
}
