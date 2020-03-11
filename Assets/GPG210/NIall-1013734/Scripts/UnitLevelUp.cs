using System.Collections;
using System.Collections.Generic;
using GPG220.Luca.Scripts.Abilities;
using UnityEngine;

public class UnitLevelUp : AbilityBase
{
    public int Kills;
    public Material[] material;
    private Renderer rend;
    public GameObject outerProng;
    public GameObject centreProng;
    


    public void Start()
    {
        Kills = 0;
        rend = GetComponent<Renderer>();
        rend.enabled = true;
        rend.sharedMaterial = material[0];

    }

    public enum KillState
    {
        LevelOne,
        LevelTwo,
        LevelThree

    }

    public KillState currentKillState;
    
    public void Update()
    {
        switch (currentKillState)
        {
            case KillState.LevelOne:

                rend.sharedMaterial = material[0];
                
                outerProng.SetActive(false);
                centreProng.SetActive(true);
                
                if (Kills >= 2 && Kills <= 3)
                {
                    currentKillState = KillState.LevelTwo;

                }
            

                break;

            case KillState.LevelTwo:
                
                rend.sharedMaterial = material[1];
                
              
                outerProng.SetActive(true);
                centreProng.SetActive(false);
                if (Kills >= 4)
                {
                    currentKillState = KillState.LevelThree;
                    
               
                }
            

                break;
            
            case KillState.LevelThree:
                
                rend.sharedMaterial = material[2];
                
                outerProng.SetActive(true);
                centreProng.SetActive(true);
                
                break;
        }
    }

    public override bool Execute(GameObject executorGameObject, GameObject[] targets = null)
    {
        
        
        
        
        
        return true;
    }
}