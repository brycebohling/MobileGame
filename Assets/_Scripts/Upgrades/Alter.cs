using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Alter : MonoBehaviour, IInteractable
{
    public void OnInteract()
    {
        UpgradeManager.upgradeManager.SpawnUpgrades();
    }
}
