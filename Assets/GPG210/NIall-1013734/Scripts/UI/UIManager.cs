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

    private void ClickedButton()
    {
        Debug.Log("yEYEEE");
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
            var abilityControllerAbilities = ((UnitBase) selectables[0]).abilityController.abilities;

            int counter = 0;
            foreach (var item in abilityControllerAbilities)
            {
                counter++;

                Debug.Log(item.Value.abilityName);
                Debug.Log(item.Value.abilityDescription);

                buttons[counter].GetComponentInChildren<TextMeshProUGUI>().text = item.Value.abilityName;
                buttons[counter].onClick.AddListener(ClickedButton);
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
        
        abilitySelectionUI.SetActive(true);
        var abilityControllerAbilities = ((UnitBase) selectables[0]).abilityController.abilities;

        int counter = 0;
        foreach (var item in abilityControllerAbilities)
        {
            counter++;

            Debug.Log(item.Value.abilityName);
            Debug.Log(item.Value.abilityDescription);

            buttons[counter].GetComponentInChildren<TextMeshProUGUI>().text = "";
            buttons[counter].onClick.RemoveListener(ClickedButton);
        }
    }
}