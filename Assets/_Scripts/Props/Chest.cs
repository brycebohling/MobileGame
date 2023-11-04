using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Chest : MonoBehaviour, IInteractable, IMultiDirectionalProp
{
    public UnityEvent OnOpen;

    [SerializeField] PropSO propSO;

    Animator animator;
    SpriteRenderer spriteRenderer;


    private void Awake() 
    {
        spriteRenderer = gameObject.GetComponentInChildren<SpriteRenderer>();

        if (spriteRenderer.TryGetComponent(out Animator anim))    
        {
            animator = anim;
            animator.enabled = false;
        }
    }

    public void OnInteract()
    {
        Open();
    }

    private void Open()
    {
        if (animator == null) 
        {
            Debug.LogWarning("Animator null");   
            return;
        }
        
        foreach (PropSO.PropGraphicsData graphic in propSO.PropGraphics)
        {
            if (graphic.sprite == spriteRenderer.sprite)
            {
                animator.enabled = true;
                Helpers.ChangeAnimationState(animator, graphic.animationClip.name);
            }
        }
        
        OnOpen?.Invoke();

        Destroy(this);
    }

    public void InitPropSprite(Sprite sprite)
    {
        spriteRenderer.sprite = sprite;
    }
}
