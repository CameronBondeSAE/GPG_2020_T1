using System;
using System.Collections;
using System.Collections.Generic;
using GPG220.Luca.Scripts.Unit;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public List<UnitBase> enemyUnitBases = new List<UnitBase>();
   public List<UnitBase> playerUnitBases = new List<UnitBase>();

   public event Action gameOverEvent;
   public PlayMenu playMenu;
   
   

   private void Start()
   {
       UnitBase.SpawnStaticEvent += UnitBaseOnSpawnStaticEvent;
       UnitBase.DespawnStaticEvent += UnitBaseOnDespawnStaticEvent;
       Health.deathStaticEvent += HealthOndeathStaticEvent;
       playMenu.playEvent += PlayMenuOnplayEvent;
   }

   private void PlayMenuOnplayEvent()
   {
      PlayMenuOnplayEvent();
   }

   private void HealthOndeathStaticEvent()
   {
       if (gameObject.GetComponent<Health>().CurrentHealth <= 0)
       {
           HealthOndeathStaticEvent();
       }
      
   }

   private void UnitBaseOnDespawnStaticEvent(UnitBase obj)
   {
       playerUnitBases.Remove(obj);
       enemyUnitBases.Remove(obj);
   }

   private void UnitBaseOnSpawnStaticEvent(UnitBase obj)
   {
       // TODO Define Enemies and Players
       playerUnitBases.Add(obj);
       enemyUnitBases.Add(obj);
   }

   public void CheckIfGameOver()
   {
       if (playerUnitBases.Count == 0 || enemyUnitBases.Count == 0 && gameOverEvent != null)
       {
           gameOverEvent.Invoke();
       }
   }


    

  
    

    
}
