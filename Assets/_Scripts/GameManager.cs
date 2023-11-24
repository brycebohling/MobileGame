using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Gm { get; private set; }
    
    [Header("Time")]
    [Range(0.1f, 10f)] public float timeScale;
    public Transform playerTransfrom;


    private void Awake()
    {
        if (Gm != null && Gm != this)
        {
            Debug.LogWarning("More than one GameManager in a scene!");
        } else
        {
            Gm = this;
        }
    }

    private void Update()
    {
        Time.timeScale = timeScale;
    }

    public void UnPauseGame()
    {
        Time.timeScale = 1;
    }

    public void PauseGame()
    {
        Time.timeScale = 0;
    }
}