using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfterAction : MonoBehaviour
{
    [SerializeField] bool destroyAfterTime;
    [SerializeField] float destroyTime;

    [SerializeField] bool destroyAfterAmin;
    [SerializeField] Animator animator;
    [SerializeField] AnimationClip destroyAfterClip;

    float destroyAfterCounter;


    void Update()
    {
        if (destroyAfterTime)
        {
            destroyAfterCounter += Time.deltaTime;

            if (destroyAfterCounter >= destroyTime)
            {
                Destroy(gameObject);
            }

        } else if (destroyAfterAmin)
        {
            if (!Helpers.IsAnimationPlaying(animator, destroyAfterClip.name))
            {
                Destroy(gameObject);
            }
        }
    }
}
