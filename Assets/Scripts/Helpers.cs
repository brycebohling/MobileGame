using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Helpers
{
    public static void ChangeAnimationState(Animator animator, string stateName)
    {
        animator.Play(stateName);
    }

    public static bool IsAnimationPlaying(Animator animator, string stateName)
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName(stateName) && animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
        {
            return true;
        } else
        {
            return false;
        }
    }
}
