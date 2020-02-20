using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitLevelUp : MonoBehaviour
{
    public int Kills;
    public Material[] material;
    private Renderer rend;


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
            {
                if (Kills >= 2 && Kills <= 3)
                {
                    currentKillState = KillState.LevelTwo;
                }
            }

                break;

            case KillState.LevelTwo:
                
                rend.sharedMaterial = material[1];
                
            {
                if (Kills >= 4)
                {
                    currentKillState = KillState.LevelThree;
                }
            }

                break;
            
            case KillState.LevelThree:
                
                rend.sharedMaterial = material[2];
                
                break;
        }
    }
    
    
}