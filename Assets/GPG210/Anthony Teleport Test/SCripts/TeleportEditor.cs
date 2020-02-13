﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using GPG220.Luca.Scripts.Unit;
using UnityEngine;
using Random = UnityEngine.Random;

public class TeleportEditor : UnitBase
{
    public bool isTeleporting = true;
    public Transform teleportTarget;
    public GameObject player;
    public float Range = -10.0f;

    //moving variables
    public float moveSpeed;
    public Transform currentTarget;

    public override void OnSelected()
    {
        base.OnSelected();
       
    }

    public override void OnDeSelected()
    {
        base.OnDeSelected();
    }

    public override void OnExecuteAction(Vector3 worldPosition, GameObject g)
    {
        base.OnExecuteAction(worldPosition, g);
        
    }

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
        Invoke("DelayTeleport", 1); //delay the teleporter by 1 second
    }

    public void DelayTeleport()
    {
        player.SetActive(true);
    }

    public void StartWalk()
    {
        Debug.Log(("Player is Walking towards targets"));
        //player.transform.position = Vector3.MoveTowards(player.transform.position, currentTarget.position, moveSpeed);

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