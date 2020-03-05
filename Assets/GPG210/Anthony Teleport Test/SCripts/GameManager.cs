using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    public List<UnitBase> enemyUnitBases = new List<UnitBase>();
   public List<UnitBase> playerUnitBases = new List<UnitBase>();
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
        networkManager.OnClientConnectedEvent += NetworkManagerOnClientConnectedEvent;
   }

   private void NetworkManagerOnClientConnectedEvent(NetworkConnection conn)
   {
       if (networkManager != null)
       {
           listofPlayerBases.Add(conn.identity.GetComponent<PlayerBase>());
           conn.identity.GetComponent<PlayerBase>().BuildUnits();
       }
      
   }

   private void PlayMenuOnplayEvent()
   {
       if(startGameEvent != null)
       {
           startGameEvent.Invoke();
       }


   }
   

   private void HealthOndeathStaticEvent(Health health)
   {
      
      
   }

   private void UnitBaseOnDespawnStaticEvent(UnitBase obj)
   {
       playerUnitBases.Remove(obj);
       enemyUnitBases.Remove(obj);
       listOfSpawns = FindObjectsOfType<SpawnPoint>().ToList();
   }

   private void UnitBaseOnSpawnStaticEvent(UnitBase obj)
   {
       // TODO Define Enemies and Players
       playerUnitBases.Add(obj);
       enemyUnitBases.Add(obj);
       obj.GetComponent<Health>().deathEvent+= HealthOndeathStaticEvent;
   }

   

   public void CheckIfGameOver()
   {
       if (playerUnitBases.Count == 0 || enemyUnitBases.Count == 0 && gameOverEvent != null)
       {
           gameOverEvent.Invoke();
       }
   }


    

  
    

    
}
