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
  
    
    


   

    public void Teleporting()
    {
        Debug.Log("Teleporting Activated");
        //player.transform.position = teleportTarget.transform.position;
        player.transform.position = new Vector3(Random.Range(-10.0f, 10.0f), 0, Random.Range(-10.0f, 10.0f));
            
    }
}
