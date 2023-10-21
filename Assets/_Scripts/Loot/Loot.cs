using UnityEngine;

public class Loot : MonoBehaviour
{
    [SerializeField] LootSO lootSO;

    [Header("Conditions")]
    [Tooltip("Requires Health component")]
    [SerializeField] bool spawnLootOnDeath;
    [Tooltip("Requires Health component")]
    [SerializeField] bool spawnLootOnDamage;

    [Header("Spawn")]
    [SerializeField] Vector2Int quantity;

    Health healthScript;


    private void Awake() 
    {
        if (TryGetComponent(out Health health))
        {
            healthScript = health;
        }
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

    public void SpawnLoot()
    {
        int totalWeight = 0;

        foreach (LootObject item in lootSO.lootTable)
        {
            totalWeight += item.weight;
        }

        int randomQuantity = Random.Range(quantity.x, quantity.y + 1);

        for (int i = 0; i < randomQuantity; i++)
        {
            int randomWeight = Random.Range(1, totalWeight + 1);
            int counter = 0;

            foreach (LootObject item in lootSO.lootTable)
            {
                counter += item.weight;

                if (counter >= randomWeight)
                {
                    Instantiate(item.gameObject, transform.position, Quaternion.identity);
                }
            }
        }
    }
}
