using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;


#if UNITY_EDITOR
using UnityEditor;
#endif

public class Health : MonoBehaviour
{
    public UnityEvent OnDeathEvent;
    public event Action OnDeath;
    public event Action<float, float, float, Vector2> OnDamaged;
    public event Action<float, float, float> OnHeal;
    
    public float maxHealth;
    [SerializeField] float startingHealth;
    [SerializeField] float IFrames;

    [SerializeField] public bool healthAsInt;

    [Header("Custom Inspecter")]
    [SerializeField] public bool takeAConstantDamage;
    [HideInInspector] public float constantDamage;

    float currentHealth;
    float currentIFrames;
    bool isInvincible;
    bool isDead;


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

    private void Update()
    {
        HandleTimers();
    }

    public void Damage(float dmg, float knockBackForce, Vector2 senderPos)
    {
        if (isDead) return;

        if (healthAsInt)
        {
            dmg = Mathf.RoundToInt(dmg);
        }

        if (takeAConstantDamage)
        {
            if (currentIFrames >= IFrames)
            {
                currentHealth -= constantDamage;

                if (currentHealth <= 0)
                {
                    Death();

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
                Death();
            } else
            {
                currentIFrames = 0;

                OnDamaged?.Invoke(dmg, currentHealth, knockBackForce, senderPos);
            }
        }
    }

    public void InstaKill()
    {
        if (isDead) return;
        
        currentHealth = 0;

        Death();
    }

    private void Death()
    {
        isDead = true;
        OnDeath?.Invoke();
        OnDeathEvent?.Invoke();
    }

    public void Heal(float healAmount)
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

        OnHeal?.Invoke(healAmount, currentHealth, maxHealth);
    }

    private void HandleTimers()
    {
        currentIFrames += Time.deltaTime;
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(Health))]
public class HealthEditor : Editor
{
	public override void OnInspectorGUI()
	{
		DrawDefaultInspector();

		Health healthScript = (Health)target;

		if (healthScript.takeAConstantDamage)
		{
			healthScript.constantDamage = EditorGUILayout.FloatField("Constant Damage", healthScript.constantDamage);
		}
	}
}
#endif