using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager gm { get; private set; }
    public Transform playerTransfrom;


    void Awake()
    {
        if (gm != null && gm != this)
        {
            Debug.Log("More than one GameManager in a scene!");
        } else
        {
            gm = this;
        }
    }
}
