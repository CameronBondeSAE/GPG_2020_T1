using System.Collections;
using System.Collections.Generic;
using GPG220.Luca.Scripts.Resources;
using TMPro;
using UnityEngine;

public class ResourceUI : MonoBehaviour
{
    public Inventory kingInv;

    public TextMeshProUGUI TMP;
    public GameObject panel;
    private GameManager gameManager;
    public ResourceType resourceType;
    
    // Start is called before the first frame update
    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        gameManager.StartGameEvent += gameStart;
        panel.SetActive(false);
    }
    

    public void gameStart()
    {
        StartCoroutine(DelayedKingInvCheck()); // Hack TODO: Maybe have an event invoked after a king is spawned.
    }

    private IEnumerator DelayedKingInvCheck()
    {
        yield return new WaitForSeconds(1f);
        kingInv = gameManager.localPlayer.king.inventory;
        kingInv.ResQuantityChangedEvent += UpdateResCount;
        TMP.text = "Gold : " + kingInv.GetResourceQuantity(resourceType);
        panel.SetActive(true);
    }

    public void UpdateResCount(Inventory inventory, ResourceType _resourceType, int amtChange)
    {
        if (_resourceType == resourceType)
        {
            TMP.text = "Gold : " + inventory.GetResourceQuantity(resourceType);
        }
    }

}


