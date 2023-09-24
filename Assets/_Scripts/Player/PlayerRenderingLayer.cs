using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRenderingLayer : MonoBehaviour
{
    SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    void Update()
    {
        spriteRenderer.sortingOrder = Mathf.RoundToInt(-transform.position.y);
    }
}
