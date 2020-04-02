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

    public Button[] buttons;
    public GameObject abilitySelectionUI;

    public void Start()
    {
        unitSelectionManager = FindObjectOfType<UnitSelectionManager>();
        unitSelectionManager.onSelectionEvent += OnSelection;
        unitSelectionManager.onDeselectionEvent += OnDeselection;
        Debug.Log(units.Count.ToString());
        abilitySelectionUI.SetActive(false);
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
            abilitySelectionUI.SetActive(true);

            int counter = 0;
            foreach (Button button in buttons)
            {
                button.gameObject.SetActive(false);
            }


            var abilityControllerAbilities = ((UnitBase) selectables[0]).abilityController.abilities;


            foreach (var item in abilityControllerAbilities)
            {
                buttons[counter].gameObject.SetActive(true);
                buttons[counter].GetComponentInChildren<TextMeshProUGUI>().text = item.Value.abilityName;
                buttons[counter].GetComponent<AbilityButton>().abilityBase = item.Value;
                buttons[counter].GetComponent<AbilityButton>().index = counter;

                counter++;
            }
        }
    }

    private void OnDeselection(List<ISelectable> selectables)
    {
        // Removes UI when no Unit is Selected.
        if (selectables.Count <= 0)
        {
            abilitySelectionUI.SetActive(false);
        }
    }
}