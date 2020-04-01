﻿using System.Collections;
using System.Collections.Generic;
using GPG220.Luca.Scripts.Abilities;
using UnityEngine;

public class AbilityButton : MonoBehaviour
{

    public AbilityBase abilityBase;
    public int index;


    public void ClickedButton()
    {
        if (abilityBase != null)
        {
         //   abilityBase.SelectedExecute();
         abilityBase.GetComponent<AbilityController>().SelectedExecuteAbility(index);
        }
    }


}
