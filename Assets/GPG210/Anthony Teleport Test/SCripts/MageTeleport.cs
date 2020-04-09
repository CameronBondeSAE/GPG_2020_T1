using System;
using System.Collections;
using System.Collections.Generic;
using GPG220.Luca.Scripts.Abilities;
using UnityEngine;
using UnityEngine.Serialization;

public class MageTeleport : AbilityBase
{
    public bool isTeleporting = true;
    public Transform teleportTarget;
    public GameObject player;
    public float Range = -10.0f;
    public GameObject visuals;
    public GameObject teleportEffectPrefab;

    //moving variables
    public float moveSpeed;
    public Transform currentTarget;
    public int damage;

    public float teleportTimer;
    public float timeLeft;

    private Rigidbody rb;
    public Vector3 offset;
   
    private void Awake()
    {
        abilityName = "Teleport";
        abilityDescription = "Teleport to another player on the map killing them instantly.";
        targetRequired = true;

    }

    

    public override bool TargetExecute(Vector3 worldPos)
    {
        Debug.Log("Teleporting Activated");
        teleportTimer = Vector3.Distance(transform.position,worldPos+= offset)/10f;
        transform.position = worldPos += offset;
        ///TODO
        ///Collider Bounds
        
        
        //Raycasting and Offsets to not teleport through the floor
        
        //player.transform.position = new Vector3(Random.Range(-Range, Range), 1, Random.Range(-Range, Range));
        visuals.SetActive(false);
        StartCoroutine(DelayNumerator());
        return true;
        
        return base.TargetExecute(worldPos);
    }


    public void DelayTeleport()
    {
        visuals.SetActive(true);
    }

    IEnumerator DelayNumerator()
    {
        DelayTeleport();
        var instantiate = Instantiate(teleportEffectPrefab, transform.position, Quaternion.identity);
        Destroy(instantiate,4f);
        yield return new WaitForSeconds(teleportTimer);
        
    }
    
    
}
