using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputReader : MonoBehaviour
{
    [SerializeField] InputReaderSO inputReaderSO;


    private void OnEnable() 
    {
        inputReaderSO.InteractEvent += Interact;    
    }

    private void OnDisable()
    {
        inputReaderSO.InteractEvent -= Interact;
    }

    private void Interact()
    {   
        
    }

}
