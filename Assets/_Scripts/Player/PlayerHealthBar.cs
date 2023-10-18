using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthBar : MonoBehaviour
{
    [SerializeField] Health healthScript;
    [SerializeField] Image healthBarFill;
    [SerializeField] Image healthBarFillBackground;
    [SerializeField] float lerpSpeed;

    bool isBackgroundLerping;


    private void OnEnable() 
    {
        healthScript.OnDamaged += UpdateHealthBar;
        healthScript.OnDeath += Death;
    }

    private void OnDisable() 
    {
        healthScript.OnDamaged -= UpdateHealthBar;
        healthScript.OnDeath -= Death;
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

    private void UpdateHealthBar(float dmg, float currentHealth, float knockBackForce, Vector2 senderPos)
    {
        healthBarFill.fillAmount = currentHealth / healthScript.maxHealth;

        isBackgroundLerping = true;
    }

    private void Death()
    {
        healthBarFill.fillAmount = 0;
    }
}
