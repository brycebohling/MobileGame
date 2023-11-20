using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrelPosion : MonoBehaviour
{
    [SerializeField] ParticleSystem brakeParticles;

    public void OnBarrelBrake()
    {
        Instantiate(brakeParticles, transform.position, Quaternion.identity);
    }
}
