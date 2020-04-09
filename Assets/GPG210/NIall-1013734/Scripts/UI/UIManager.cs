using System;
using System.Collections;
using System.Collections.Generic;
using GPG220.Blaide_Fedorowytsch.Scripts;
using GPG220.Blaide_Fedorowytsch.Scripts.Interfaces;
using GPG220.Luca.Scripts.Abilities;
using GPG220.Luca.Scripts.Unit;
using Sirenix.Utilities;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
    /// Having selected unit's abilities pop up on the UI grid

{
    // added by blaide, Control stuff for input events using the new unity input system.
    public InputActionAsset controls;
    [HideInInspector]
    public InputActionMap actionMap;
    [HideInInspector]
    public InputAction actionKey;
    [HideInInspector]
    public InputAction cursorMove;
    private Camera mainCam;
    
    
    
    
    
    
    private UnitSelectionManager unitSelectionManager;
    public List<UnitBase> units;

    public Button[] buttons;
    public GameObject abilitySelectionUI;

    public AbilityBase selectedTargetAbility;
    public AbilityBase selectedWorldTargetAbility;
    public AbilityController abilityController;



    private void Awake()
    {
        actionMap = controls.actionMaps[0];
        actionKey = actionMap.FindAction("ActionKeyPress");
        cursorMove = actionMap.FindAction("CursorMove");
        actionKey.performed += TargetAction;
        cursorMove.performed += CursorMove;
        mainCam = Camera.main;
    }

    public void TargetAction(InputAction.CallbackContext ctx) // called on mouse right click
    {
        Vector3 worldPosition = unitSelectionManager.targetPoint; // get the worldpoint from the unitselection manager because its already raycasting to find world points.
       GameObject targetObject = unitSelectionManager.targetObject;
       
       if (targetObject == null)
       {
           abilityController.TargetExecuteAbility(selectedWorldTargetAbility,worldPosition);
           selectedWorldTargetAbility = abilityController.defaultWorldTargetAbility;
       }
       else
       {
           abilityController.TargetExecuteAbility(selectedTargetAbility,targetObject);
           selectedTargetAbility = abilityController.defaultTargetAbility;
       }
       // same as above,  unit selection manager is already raycasting on cursor move to find these.
    }
    public void CursorMove(InputAction.CallbackContext ctx) // called when cursor is moved. 
    {
        Vector2 cursorPoint = ctx.ReadValue<Vector2>(); // just the screen space cursor position

        cursorPoint = cursorPoint.Clamp(Vector2.zero, new Vector2(mainCam.scaledPixelWidth, mainCam.scaledPixelHeight)); // constrained to not include when the mouse goes off screen
    }

    public void Start()
    {
        unitSelectionManager = FindObjectOfType<UnitSelectionManager>();
        unitSelectionManager.OnSelectionEvent += OnSelection;
        unitSelectionManager.OnDeselectionEvent += OnDeselection;
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

            
            foreach (Button button in buttons)
            {
                button.GetComponent<AbilityButton>().uiManager = this;
                button.gameObject.SetActive(false);
                
            }

            abilityController = ((UnitBase) selectables[0]).abilityController;
            var abilityControllerAbilities = abilityController.abilities;
            selectedTargetAbility = abilityController.defaultTargetAbility;
            selectedWorldTargetAbility = abilityController.defaultWorldTargetAbility;
            
            int counter = 0;
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