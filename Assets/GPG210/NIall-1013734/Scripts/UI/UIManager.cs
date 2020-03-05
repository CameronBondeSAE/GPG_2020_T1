using System;
using System.Collections;
using System.Collections.Generic;
using GPG220.Blaide_Fedorowytsch.Scripts;
using GPG220.Blaide_Fedorowytsch.Scripts.Interfaces;
using GPG220.Luca.Scripts.Abilities;
using GPG220.Luca.Scripts.Unit;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
    /// Having selected unit's abilities pop up on the UI grid

{
    private UnitSelectionManager unitSelectionManager;
    public List<UnitBase> units;

    public TextMeshProUGUI[] textMeshProUguis;

    public void Start()
    {
        unitSelectionManager = FindObjectOfType<UnitSelectionManager>();
        unitSelectionManager.onSelectionEvent += OnSelection;
        Debug.Log(units.Count.ToString());
    }


    private void OnSelection(List<ISelectable> selectables)
    {
        Debug.Log(selectables.ToString());
        Debug.Log(selectables.Count.ToString());

        /*foreach (var item in selectables)
        {
            string s = ((UnitBase)item).abilityController.abilities.ToString();
            Debug.Log(s);
        }*/

        if (selectables.Count == 1)
        {
            var abilityControllerAbilities = ((UnitBase)selectables[0]).abilityController.abilities;

            int counter = 0;
            foreach (var item in abilityControllerAbilities)
            {
                counter++;
                
                Debug.Log(item.Value.abilityName);
                Debug.Log(item.Value.abilityDescription);

//                buttonItems[counter].text = abilityname;
                textMeshProUguis[counter].text = item.Value.abilityName;

            }
        }
    }
}