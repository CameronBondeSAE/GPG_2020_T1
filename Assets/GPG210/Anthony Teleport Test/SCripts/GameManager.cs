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
       //UnitBase.DespawnStaticEvent += UnitBaseOnDespawnStaticEvent;
       playMenu.playEvent += PlayMenuOnplayEvent;
   }

   private void PlayMenuOnplayEvent()
   {
      
   }

   private void HealthOndeathStaticEvent(Health health)
   {
      
      
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
       obj.GetComponent<Health>().deathEvent+= HealthOndeathStaticEvent;

   }

   public void CheckIfGameOver()
   {
       if (playerUnitBases.Count == 0 || enemyUnitBases.Count == 0 && gameOverEvent != null)
       {
           gameOverEvent.Invoke();
       }
   }


    

  
    

    // 
}
