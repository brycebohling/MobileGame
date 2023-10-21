using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour, IInteractable
{
    [SerializeField] PropSO propSO;

    Animator animator;
    SpriteRenderer spriteRenderer;


    private void Awake() 
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (TryGetComponent(out Animator anim))    
        {
            animator = anim;
        }
    }

    public void OnInteract()
    {
        Open();
    }

    private void Open()
    {
        if (animator == null) return;
        
        foreach (PropSO.PropGraphics graphic in propSO.propGraphics)
        {
            if (graphic.sprite == spriteRenderer.sprite)
            {
                Helpers.ChangeAnimationState(animator, graphic.animationClip.name);
            }
        }

        Destroy(this);
    }
}
