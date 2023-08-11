using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Health : MonoBehaviour
{
    public event Action OnDeath;

    [SerializeField] float maxHealth;
    [SerializeField] float startingHealth;
    [SerializeField] float IFrames;
    
    float currentHealth;
    float currentIFrames;
    bool isInvincible;


    void Start()
    {
        currentHealth = startingHealth;
    }

    void Update()
    {
        HandleTimers();
    }

    public void DamageObject(float dmg, Vector2 senderPos)
    {
        if (!isInvincible && currentIFrames > IFrames)
        {
            currentHealth -= dmg;

            if (currentHealth <= 0)
            {
                Death();
            } else
            {
                currentIFrames = 0;
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

    private void Death()
    {
        OnDeath?.Invoke();
    }
}
