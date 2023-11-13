using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class UpgradeManager : MonoBehaviour
{
    public static UpgradeManager upgradeManager { get; private set; }

    [SerializeField] Transform upgradesHolder;
    [SerializeField] AnimationClip fireBallAnim;

    [SerializeField] List<Transform> upgradeCards;

    bool isActive;
    bool isCardSpawned;
    float fireBallAnimCounter;


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

    private void Update()
    {
        if (isActive && !isCardSpawned)
        {
            fireBallAnimCounter += Time.unscaledDeltaTime;

            if (fireBallAnimCounter >= fireBallAnim.length)
            {
                isCardSpawned = true;
                OnCardSpawn();
            }
        }
    }

    private void OnActivate()
    {
        isActive = true;

        upgradesHolder.gameObject.SetActive(true);

        GameManager.Gm.PauseGame();
    }

    private void OnDeactivate()
    {
        isActive = false;
        isCardSpawned = false;
        fireBallAnimCounter = 0;
    }

    public void SpawnUpgrades()
    {
        OnActivate();
    }

    private void HideUpgrade()
    {
        upgradesHolder.gameObject.SetActive(false);

        GameManager.Gm.UnPauseGame();

        OnDeactivate();
    }

    private void OnCardSpawn()
    {
        EnableClicks();
    }

    private void EnableClicks()
    {
        EventTrigger.Entry clickEvent = new EventTrigger.Entry
        {
            eventID = EventTriggerType.PointerClick
        };

        clickEvent.callback.AddListener((e) => { HideUpgrade(); });
        clickEvent.callback.AddListener((e) => { DisableClicks(); });

        foreach (Transform card in upgradeCards)
        {
            EventTrigger eventTrigger = card.GetComponent<EventTrigger>();
            eventTrigger.triggers.Add(clickEvent);
        }
    }

    private void DisableClicks()
    {
        foreach (Transform card in upgradeCards)
        {
            EventTrigger eventTrigger = card.GetComponent<EventTrigger>();

            eventTrigger.triggers.RemoveRange(0, eventTrigger.triggers.Count);
        }
    }    
}
