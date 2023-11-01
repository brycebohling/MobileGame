using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfterAction : MonoBehaviour
{
    [Header("Time")]
    [SerializeField] bool destroyAfterTime;
    [SerializeField] bool fadeAfterTime;
    [SerializeField] float time;

    [Header("Animatinos")]
    [SerializeField] bool destroyAfterAmin;
    [SerializeField] bool fadeAfterAnim;
    [SerializeField] bool playAnimOnAwake;
    [SerializeField] Animator animator;
    [SerializeField] AnimationClip animClip;

    [Header("Action")]
    [SerializeField] bool fadeAfterAction;
    
    [Header("Graphics")]
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] float fadeSpeed;
    

    float timeCounter;


    private void Awake() 
    {
        if (playAnimOnAwake)
        {
            Helpers.ChangeAnimationState(animator, animClip.name);
        }
    }

    private void LateUpdate()
    {
        if (destroyAfterTime || fadeAfterTime)
        {
            time += Time.deltaTime;

            if (timeCounter >= time)
            {
                if (destroyAfterTime)
                {
                    Destroy(gameObject);

                } else if (fadeAfterTime)
                {
                    FadeAlpha();
                }
            }
        }

        if (destroyAfterAmin || fadeAfterAnim)
        {
            if (!Helpers.IsAnimationPlaying(animator, animClip.name))
            {
                if (destroyAfterAmin)
                {
                    Destroy(gameObject);
                    
                } else if (fadeAfterAnim)
                {
                    FadeAlpha();
                }
            }
        }

        if (fadeAfterAction)
        {
            FadeAlpha();
        }
    }

    private void FadeAlpha()
    {
        float newAlpha = Mathf.Lerp(spriteRenderer.color.a, 0, fadeSpeed * Time.deltaTime);
        spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, newAlpha);
    }
}
