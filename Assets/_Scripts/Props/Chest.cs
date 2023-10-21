using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{
    [SerializeField] PropSO propSO;

    [SerializeField] InputReaderSO inputReaderSO;

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

    private void OnEnable() 
    {
        inputReaderSO.InteractEvent += OnOpen;    
    }

    private void OnDisable()
    {
        inputReaderSO.InteractEvent -= OnOpen;
    }

    private void OnOpen()
    {
        if (animator == null) return;
        
        foreach (PropSO.PropGraphics graphic in propSO.propGraphics)
        {
            if (graphic.sprite == spriteRenderer.sprite)
            {
                Helpers.ChangeAnimationState(animator, graphic.animationClip.name);
            }
        }

    }
}
