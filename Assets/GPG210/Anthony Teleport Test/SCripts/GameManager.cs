using System;
using System.Collections;
using System.Collections.Generic;
using GPG220.Luca.Scripts.Unit;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : UnitBase
{
    public static int numberOfPlayers;
    public static int numberOfEnemies;

   
    
  
   public List<UnitBase> enemyUnitBases = new List<UnitBase>(numberOfEnemies);
   public List<UnitBase> playerUnitBases = new List<UnitBase>(numberOfPlayers);
  

   protected override void Initialize()
   {
       base.Initialize();
   }

  /* void Main()
   {
       UnitBase.DespawnStaticEvent += RemovePlayer  ;
   }
   public List<UnitBase> PlayerUnitBases => playerUnitBases.Remove(DespawnStaticEvent);
   static void AddPlayer()
   {
       for()
     
      
   }

    void RemovePlayer()
    {
      
       enemyUnitBases.Remove(+=DespawnStaticEvent())
   }*/
    

  
    

    
}
