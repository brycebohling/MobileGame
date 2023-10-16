using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CrosshairCursor : MonoBehaviour
{
    [SerializeField] Camera cam;
    [SerializeField] SpriteRenderer crosshairSpriteRenderer;
    [SerializeField] Sprite normalCrosshair;
    [SerializeField] Sprite fireCrosshair;

    private void Awake()
    {
        Cursor.visible = false;
    }

    private void Update()
    {
        Vector2 mousePosition = cam.ScreenToWorldPoint(Input.mousePosition);
        transform.position = mousePosition;

        if (Mouse.current.leftButton.IsPressed())
        {
            crosshairSpriteRenderer.sprite = fireCrosshair;

        } else
        {
            crosshairSpriteRenderer.sprite = normalCrosshair;
        }
    }
}
