using System.Collections;
using System.Collections.Generic;
using GPG220.Luca.Scripts.Abilities;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.Experimental.PlayerLoop;

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
        currentKillState = KillState.LevelOne;
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


                outerProng.SetActive(false);
                centreProng.SetActive(true);

                if (Kills >= 1 && Kills <= 2)
                {
                    currentKillState = KillState.LevelTwo;
                }


                break;

            case KillState.LevelTwo:


                outerProng.SetActive(true);
                centreProng.SetActive(false);
                if (Kills >= 3)
                {
                    currentKillState = KillState.LevelThree;
                }


                break;

            case KillState.LevelThree:


                outerProng.SetActive(true);
                centreProng.SetActive(true);

                break;
        }
    }

    public override bool SelectedExecute()
    {
        Kills += 1;


        return base.SelectedExecute();
    }
}