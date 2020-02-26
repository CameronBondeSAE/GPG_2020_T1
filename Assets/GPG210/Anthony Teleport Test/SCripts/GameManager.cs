using System;
using System.Collections;
using System.Collections.Generic;
using GPG220.Luca.Scripts.Unit;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : UnitBase
{
   public List<UnitBase> playerUnitBases = new List<UnitBase>(10);
   public List<UnitBase> enemyUnitBases = new List<UnitBase>(10);

   public List<UnitBase> PlayerUnitBases
   {
       get => playerUnitBases;
       set => playerUnitBases = value;
   }

   protected override void Initialize()
   {
       base.Initialize();
   }


   /*public List<UnitBase> PlayerUnitBases => playerUnitBases.Remove(DespawnStaticEvent);
   public void AddPlayer()
   {
        UnitBase.SpawnStaticEvent();
       //subscribe to the event
       playerUnitBases.Add(SpawnStaticEvent);
   }*/

  
    

    
}
