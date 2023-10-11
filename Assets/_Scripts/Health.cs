using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class Health : MonoBehaviour
{
    public event Action OnDeath;
    public event Action<float, float, float, Vector2> OnDamaged;
    
    [SerializeField] float maxHealth;
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

    public void DamageObject(float dmg, float knockBackForce, Vector2 senderPos)
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
                isDead = true;
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

#if UNITY_EDITOR
[CustomEditor(typeof(Health))]
public class HealthEditor : Editor
{
	public override void OnInspectorGUI()
	{
		DrawDefaultInspector();

		Health healthScript = (Health)target;

		// healthScript.takeAConstantDamage = EditorGUILayout.Toggle("takeAConstantDamage", healthScript.takeAConstantDamage);

		if (healthScript.takeAConstantDamage)
		{
			healthScript.constantDamage = EditorGUILayout.FloatField("Constant Damage", healthScript.constantDamage);
		}
	}
}
#endif