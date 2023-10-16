using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loot : MonoBehaviour
{
    [SerializeField] LootSO lootSO;

    [Header("Conditions")]
    [SerializeField] bool spawnLootOnDeath;
    [SerializeField] bool spawnLootOnDamage;

    [Header("Spawn")]
    [SerializeField] bool canSpawn = true;
    [SerializeField] float delay;
    [SerializeField] int quantity;

    
}
