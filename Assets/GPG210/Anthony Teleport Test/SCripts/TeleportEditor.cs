using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using Random = UnityEngine.Random;

public class TeleportEditor : MonoBehaviour
{
    public bool isTeleporting = true;
    public Transform teleportTarget;
    public GameObject player;
    public float Range = -10.0f;
    
    //moving variables
    public float moveSpeed;
    public Transform currentTarget;
   

    Enemy closestEnemy = null;
    void Update()
    {
       
        
    }


    public void Teleporting()
    {
        Debug.Log("Teleporting Activated");
        //player.transform.position = teleportTarget.transform.position;
        
        player.transform.position = new Vector3(Random.Range(-Range, Range), 1, Random.Range(-Range, Range));
        player.SetActive(false);
        Invoke("DelayTeleport",1); //delay the teleporter by 1 second
     
     

    }

    public void DelayTeleport()
    {
        player.SetActive(true);
    }

    public void StartWalk()
    {
        Debug.Log(("Player is Walking towards targets"));
        WalkToClosest();
    }
    void WalkToClosest()
    {
        float distanceToClosestEnemy = Mathf.Infinity;
      
        Enemy[] allEnemies = GameObject.FindObjectsOfType<Enemy>();

        foreach (Enemy currentEnemy in allEnemies)
        {
            float distanceToEnemy = (currentEnemy.transform.position - this.transform.position).sqrMagnitude;
            if (distanceToEnemy < distanceToClosestEnemy)
            {
                distanceToClosestEnemy = distanceToEnemy;
                closestEnemy = currentEnemy;
               
               
            }
        }



    }
}
