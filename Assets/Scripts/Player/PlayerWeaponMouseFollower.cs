using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerWeaponMouseFollower : PlayerBase
{
    [Header("Blocking States")]
    public PlayerStates.States[] BlockingActionStates;
    
    [Header("Weapon")]
    [SerializeField] Transform weaponPivot;

    Vector3 mouseLocalPosition;
    Vector3 mouseWorldPosition;

    bool isWeaponOnTheRight = true;

    protected override void Start()
    {
        base.Start();
    }

    void Update()
    {
        if (!IsActionAuth(BlockingActionStates)) return;
        
        ApplyAction();
    }

    protected override void ApplyAction()
    {
        mouseLocalPosition = Mouse.current.position.ReadValue();
        mouseWorldPosition = Camera.main.ScreenToWorldPoint(mouseLocalPosition);

        Vector3 mouseRelativeToPlayPosition = mouseWorldPosition - _playerSpriteSpriteRenderer.transform.position;
        float angle = Mathf.Atan2(mouseRelativeToPlayPosition.y, mouseRelativeToPlayPosition.x) * Mathf.Rad2Deg;

        if (isWeaponOnTheRight)
        {
            if (Mathf.Abs(weaponPivot.localEulerAngles.z) < 90 || weaponPivot.localEulerAngles.z > 270)
            {
        
                weaponPivot.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

            } else
            {
                FlipWeapon();
               
                weaponPivot.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

                Vector3 weaponLocalEulerAngles = weaponPivot.localEulerAngles;
                weaponLocalEulerAngles.z -= 180;    
                weaponPivot.localEulerAngles = weaponLocalEulerAngles;
            }

        } else
        {
            if (Mathf.Abs(weaponPivot.localEulerAngles.z) < 90 || weaponPivot.localEulerAngles.z > 270)
            {
        
                weaponPivot.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

                Vector3 weaponLocalEulerAngles = weaponPivot.localEulerAngles;
                weaponLocalEulerAngles.z -= 180;    
                weaponPivot.localEulerAngles = weaponLocalEulerAngles;

            } else
            {
                FlipWeapon();
               
                weaponPivot.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            }
        }
        
    }

    private void FlipWeapon()
    {
        Vector3 weaponLocalScale = weaponPivot.localScale;
        weaponLocalScale.x *= -1;    
        weaponPivot.localScale = weaponLocalScale;

        isWeaponOnTheRight = !isWeaponOnTheRight;
    }
}