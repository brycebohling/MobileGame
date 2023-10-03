using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Health : MonoBehaviour
{
    public event Action OnDeath;
    public event Action<float, float, float, Vector2> OnDamaged;
    
    [SerializeField] float maxHealth;
    [SerializeField] float startingHealth;
    [SerializeField] float IFrames;

    [SerializeField] bool healthAsInt;
    [SerializeField] bool takeAConstantDamge;
    [SerializeField] float constantDamage;
    
    float currentHealth;
    float currentIFrames;
    bool isInvincible;


    void Start()
    {
        if (healthAsInt)
        {
            maxHealth = Mathf.RoundToInt(maxHealth);
            startingHealth = Mathf.RoundToInt(startingHealth);
            constantDamage = Mathf.RoundToInt(constantDamage);
        }

        currentHealth = startingHealth;
        currentIFrames = IFrames;
    }

    void Update()
    {
        HandleTimers();
    }

    public void DamageObject(float dmg, float knockBackForce, Vector2 senderPos)
    {
        if (healthAsInt)
        {
            dmg = Mathf.RoundToInt(dmg);
        }

        if (takeAConstantDamge)
        {
            if (currentIFrames >= IFrames)
            {
                currentHealth -= constantDamage;

                if (currentHealth <= 0)
                {
                    OnDeath?.Invoke();

                } else
                {
                    currentIFrames = 0;

                    OnDamaged?.Invoke(dmg, currentHealth, knockBackForce, senderPos);
                }
            }

            return;
        }

        if (!isInvincible && currentIFrames >= IFrames)
        {
            currentHealth -= dmg;

            if (currentHealth <= 0)
            {
                OnDeath?.Invoke();

            } else
            {
                currentIFrames = 0;

                OnDamaged?.Invoke(dmg, currentHealth, knockBackForce, senderPos);
            }
        }
    }

    private void Heal(float healAmount)
    {
        if (healthAsInt)
        {
            healAmount = Mathf.RoundToInt(healAmount);
        }

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
