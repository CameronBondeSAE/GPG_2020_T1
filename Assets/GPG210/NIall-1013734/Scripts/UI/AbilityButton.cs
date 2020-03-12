using System.Collections;
using System.Collections.Generic;
using GPG220.Luca.Scripts.Abilities;
using UnityEngine;

public class AbilityButton : MonoBehaviour
{

    public AbilityBase abilityBase;


    public void ClickedButton()
    {
        if (abilityBase != null)
        {
            abilityBase.SelectedExecute();
        }
    }


}
