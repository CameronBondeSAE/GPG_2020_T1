using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using GPG220.Blaide_Fedorowytsch.Scripts;
using GPG220.Luca.Scripts.Unit;
using Mirror;
using Mirror.Examples.Basic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Random = System.Random;


public class GameManager : MonoBehaviour
{
    public List<UnitBase> globalUnitBases = new List<UnitBase>();
    public List<SpawnPoint> listOfSpawns = new List<SpawnPoint>();
    public List<PlayerBase> listofPlayerBases = new List<PlayerBase>();

    public RTSNetworkManager networkManager;

    public event Action gameOverEvent;
    public event Action startGameEvent;

    public PlayMenu playMenu;

    


    private void Start()
    {
        UnitBase.SpawnStaticEvent += UnitBaseOnSpawnStaticEvent;
        UnitBase.DespawnStaticEvent += UnitBaseOnDespawnStaticEvent;
        playMenu.playEvent += PlayMenuOnplayEvent;
        networkManager.OnClientPlayerSpawnEvent += NetworkManagerOnOnClientPlayerSpawnEvent;
        networkManager.OnClientDisconnectedEvent += NetworkManagerOnOnClientDisconnectedEvent;
        
        
    }

    private void NetworkManagerOnOnClientDisconnectedEvent(NetworkConnection conn)
    {
        if (networkManager != null)
        {
            listofPlayerBases.Remove(conn.identity.GetComponent<PlayerBase>());
            //conn.identity.GetComponent<PlayerBase>().BuildUnits();
            //conn.identity.GetComponent<UnitBase>().owner.units
        }
    }

    private void NetworkManagerOnOnClientPlayerSpawnEvent(NetworkConnection conn)
    {
        if (networkManager != null)
        {
            listofPlayerBases.Add(conn.identity.GetComponent<PlayerBase>());
            conn.identity.GetComponent<PlayerBase>().BuildUnits();
        }
    }

    private void PlayMenuOnplayEvent()
    {
        if (startGameEvent != null)
        {
            startGameEvent.Invoke();
            
        }
        
    }


    private void HealthOndeathStaticEvent(Health health)
    {
    }

    private void UnitBaseOnDespawnStaticEvent(UnitBase obj)
    {
        globalUnitBases.Remove(obj);
        obj.GetComponent<Health>().deathEvent -= HealthOndeathStaticEvent;
    }

    private void UnitBaseOnSpawnStaticEvent(UnitBase obj)
    {
        // TODO Define Enemies and Players
        globalUnitBases.Add(obj);
        obj.GetComponent<Health>().deathEvent += HealthOndeathStaticEvent;
        listOfSpawns = FindObjectsOfType<SpawnPoint>().ToList();
       
        //listOfSpawns = globalUnitBases[(Random.Range(0,listOfSpawns.Count))];
    }


    public void CheckIfGameOver()
    {
        if (globalUnitBases.Count == 0 && gameOverEvent != null)
        {
            gameOverEvent.Invoke();
           
        }
    }
}