using UnityEngine;

public class HealthLoot : MonoBehaviour
{
    
    [Header("Health")]
    [SerializeField] float healAmount;


    public void HealthCollected(Health healthScript)
    {
        healthScript.Heal(healAmount);

        Destroy(gameObject);
    }
}
