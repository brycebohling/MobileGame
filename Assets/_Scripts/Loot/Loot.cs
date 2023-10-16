using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
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
    [SerializeField] Vector2 quantity;

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
        int totalWeight = 0;

        foreach (LootObject item in lootSO.lootTable)
        {
            totalWeight += item.weight;
        }

        int randomWeight = Random.Range(1, totalWeight + 1);
        int counter = 0;

        foreach (LootObject item in lootSO.lootTable)
        {
            counter += item.weight;

            if (counter >= randomWeight)
            {
                if (item.gameObject.TryGetComponent(out ILoot lootScript))
                {
                    lootScript.Spawn();
                    break;

                } else
                {
                    Debug.LogError("Loot object did not have ILoot component attached!");
                }
            }
        }
    }
}
