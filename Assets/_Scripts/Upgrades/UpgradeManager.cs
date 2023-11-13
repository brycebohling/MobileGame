using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
    public static UpgradeManager upgradeManager { get; private set; }


    [SerializeField] AnimationClip fireBallAnim;

    [SerializeField] Transform upgradesHolder;


    private void Awake()
    {
        if (upgradeManager != null && upgradeManager != this)
        {
            Debug.LogWarning("More than one UpgradeManager in a scene!");
        } else
        {
            upgradeManager = this;
        }
    }

    public void SpawnUpgrades()
    {
        upgradesHolder.gameObject.SetActive(true);
    }
    
    public void OnCardSpawn(GameObject gameObject)
    {
        Debug.Log("OnCardSpawn");
    }

    public void HideUpgrades()
    {
        upgradesHolder.gameObject.SetActive(false);
    }
}
