﻿using System.Collections;
using System.Collections.Generic;
using GPG220.Luca.Scripts.Abilities;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Experimental.Rendering;

public class AbilityButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public UIManager uiManager;
    public AbilityBase abilityBase;
    public int index;
    public GameObject DescriptionPanel;
    public TextMeshProUGUI tmpUGUI;


    public void ClickedButton()
    {
        if (abilityBase != null)
        {
            abilityBase.GetComponent<AbilityController>().SelectedExecuteAbility(abilityBase);

            if (abilityBase.targetRequired == true)
            {
                uiManager.selectedTargetAbility = abilityBase;
                uiManager.selectedWorldTargetAbility = abilityBase;
            }
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        DescriptionPanel.SetActive(true);
        tmpUGUI.text = abilityBase.Description;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        tmpUGUI.text = "";
        DescriptionPanel.SetActive(false);
    }
}