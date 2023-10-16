using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Health))]
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

    Health healthScript;


    private void Awake() 
    {
        healthScript = GetComponent<Health>();
    }

    private void OnEnable() 
    {
        if (spawnLootOnDamage) healthScript.OnDamaged += SpawnLootOnDmg;

        if (spawnLootOnDeath) healthScript.OnDeath += SpawnLoot;
    }

    private void OnDisable() 
    {
        if (spawnLootOnDamage) healthScript.OnDamaged -= SpawnLootOnDmg;

        if (spawnLootOnDeath) healthScript.OnDeath -= SpawnLoot;
    }

    private void SpawnLootOnDmg(float dmg, float currentHealth, float knockBackForce, Vector2 senderPos)
    {
        SpawnLoot();
    }

    private void SpawnLoot()
    {
        
    }
}
