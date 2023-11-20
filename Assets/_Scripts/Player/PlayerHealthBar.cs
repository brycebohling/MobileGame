using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthBar : MonoBehaviour
{
    [SerializeField] Health healthScript;
    [SerializeField] Image healthBarFill;
    [SerializeField] Image healthBarFillBackground;
    [SerializeField, Range(0, 1)] float lerpSpeed;

    bool isBackgroundLerping;


    private void OnEnable() 
    {
        healthScript.OnDamaged += WhenDamaged;
        healthScript.OnDeath += WhenDead;
        healthScript.OnHeal += WhenHealed;
    }

    private void OnDisable() 
    {
        healthScript.OnDamaged -= WhenDamaged;
        healthScript.OnDeath -= WhenDead;
        healthScript.OnHeal -= WhenHealed;
    }

    private void FixedUpdate()
    {
        if (isBackgroundLerping)
        {
            healthBarFillBackground.fillAmount = Mathf.Lerp(healthBarFillBackground.fillAmount, healthBarFill.fillAmount, lerpSpeed);

            if (healthBarFillBackground.fillAmount - healthBarFill.fillAmount < 0.001f)
            {
                healthBarFillBackground.fillAmount = healthBarFill.fillAmount;

                isBackgroundLerping = false;
            }
        }
    }

    private void UpdateHealthBar(float currentHealth)
    {
        healthBarFill.fillAmount = currentHealth / healthScript.maxHealth;

        isBackgroundLerping = true;
    }

    private void WhenDamaged(float dmg, float currentHealth, float knockBackForce, Vector2 senderPos)
    {
        UpdateHealthBar(currentHealth);
    }

    private void WhenHealed(float healAmount, float currentHealth, float maxHealth)
    {
        UpdateHealthBar(currentHealth);
    }

    

    private void WhenDead()
    {
        UpdateHealthBar(0);
    }
}
