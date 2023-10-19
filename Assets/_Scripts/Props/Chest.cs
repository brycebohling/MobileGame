using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{
    [SerializeField] AnimationClip openAnim;

    [SerializeField] InputReaderSO inputReaderSO;

    Animator animator;


    private void Awake() 
    {
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

        Helpers.ChangeAnimationState(animator, openAnim.name);
    }
}
