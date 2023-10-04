using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Gm { get; private set; }
    public Transform playerTransfrom;


    void Awake()
    {
        if (Gm != null && Gm != this)
        {
            Debug.Log("More than one GameManager in a scene!");
        } else
        {
            Gm = this;
        }
    }
}
