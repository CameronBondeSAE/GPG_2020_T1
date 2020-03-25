using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillAmountScript : MonoBehaviour
{
    public UnitLevelUp unitLevelUp;


    void awake()
    {
    unitLevelUp = GetComponent<UnitLevelUp>();
    }

    public void IncreaseKills()
    {
        unitLevelUp.Kills += 1;
    }
    public void DecreaseKills()
    {
        unitLevelUp.Kills -= 1;
    }
}
