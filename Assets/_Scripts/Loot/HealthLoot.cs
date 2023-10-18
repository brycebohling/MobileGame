using UnityEngine;

public class HealthLoot : MonoBehaviour
{
    
    [Header("Health")]
    [SerializeField] float healAmount;


    public void HealthCollected()
    {
        GameManager.Gm.playerTransfrom.GetComponent<Health>().Heal(healAmount);

        Destroy(gameObject);
    }
}
