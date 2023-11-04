using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barrel : MonoBehaviour
{
    SpriteRenderer spriteRenderer;
    [System.Serializable] public struct BarrelVariant
    {
        public Sprite sprite;
        public AbstractBarrel script;
    }

    [SerializeField] List<BarrelVariant> barrelVariants;


    private void Start()
    {
        spriteRenderer = gameObject.GetComponentInChildren<SpriteRenderer>();
    }

    public void OnBarrelBrake()
    {
        foreach (BarrelVariant variant in barrelVariants)
        {
            if (variant.sprite == spriteRenderer.sprite)
            {
                variant.script.SpawnBarrelInsides(transform.position);
            }
        }
    }


}