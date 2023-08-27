using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[RequireComponent(typeof(Rigidbody2D))]
public class Health : MonoBehaviour
{
    public event Action OnDeath;
    public event Action<float, Vector2> OnDamaged;

    [SerializeField] float maxHealth;
    [SerializeField] float startingHealth;
    [SerializeField] float IFrames;
    
    float currentHealth;
    float currentIFrames;
    bool isInvincible;


    void Start()
    {
        currentHealth = startingHealth;
        currentIFrames = IFrames;
    }

    void Update()
    {
        HandleTimers();
    }

    public void DamageObject(float dmg, float knockBackForce, Vector2 senderPos)
    {
        if (!isInvincible && currentIFrames >= IFrames)
        {
            currentHealth -= dmg;

            if (currentHealth <= 0)
            {
                OnDeath?.Invoke();

            } else
            {
                currentIFrames = 0;

                OnDamaged?.Invoke(knockBackForce, senderPos);
            }
        }
    }

    private void Heal(float healAmount)
    {
        currentHealth += healAmount;

        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
    }

    private void HandleTimers()
    {
        currentIFrames += Time.deltaTime;
    }
}
