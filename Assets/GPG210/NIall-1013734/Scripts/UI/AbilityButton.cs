using System.Collections;
using System.Collections.Generic;
using GPG220.Luca.Scripts.Abilities;
using UnityEngine;

public class AbilityButton : MonoBehaviour
{
    public UIManager uiManager;
    public AbilityBase abilityBase;
    public int index;


    public void ClickedButton()
    {
        if (abilityBase != null)
        {
         //   abilityBase.SelectedExecute();
         abilityBase.GetComponent<AbilityController>().SelectedExecuteAbility(abilityBase);

         if (abilityBase.targetRequired == true)
         {
             uiManager.selectedTargetAbility = abilityBase;
             uiManager.selectedWorldTargetAbility = abilityBase;
         }
         
        }
        
    }


}
