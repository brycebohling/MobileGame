using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputController : MonoBehaviour
{
    [SerializeField] InputActionAsset inputActionsAsset;
    InputActionMap inputActionMap;
    InputAction dash;

    private void Start() 
    {
        inputActionMap = inputActionsAsset.FindActionMap("Player");

        dash = inputActionMap.FindAction("Dash");

        dash.performed += Temp;
    }

    private void Temp(InputAction.CallbackContext ctx)
    {
        Debug.Log("d");
    }
}
