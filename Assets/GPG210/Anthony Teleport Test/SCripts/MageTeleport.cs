using System;
using System.Collections;
using System.Collections.Generic;
using GPG220.Luca.Scripts.Abilities;
using UnityEngine;

public class MageTeleport : AbilityBase
{
    public bool isTeleporting = true;
    public Transform teleportTarget;
    public GameObject player;
    public float Range = -10.0f;

    //moving variables
    public float moveSpeed;
    public Transform currentTarget;
    public int damage;

    private Rigidbody rb;
    // Start is called before the first frame update
    private void Awake()
    {
        abilityName = "Teleport";
        abilityDescription = "Teleport to another player on the map killing them instantly.";
    }

    // Update is called once per frame
   

    public override bool Execute(GameObject executorGameObject, GameObject[] targets = null)
    {
        Debug.Log("Teleporting Activated");
        executorGameObject.transform.position = currentTarget.transform.position;

        //player.transform.position = new Vector3(Random.Range(-Range, Range), 1, Random.Range(-Range, Range));
        executorGameObject.SetActive(false);
        Invoke("DelayTeleport", 1); //delay the teleporter by 1 second
        StartCoroutine(DelayNumerator());
        return true;
    } 
    

    public void DelayTeleport()
    {
        player.SetActive(true);
    }

    IEnumerator DelayNumerator()
    {
        DelayTeleport();
        yield return new WaitForSeconds(1);
        
    }
    
}
