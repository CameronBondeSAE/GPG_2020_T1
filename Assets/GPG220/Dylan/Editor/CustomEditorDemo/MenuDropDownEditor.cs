using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class MenuDropDownEditor
{
    //to create a new menu called tools with a function called clear player prefs
    //symbols at the end indicate the shortcuts that can be used along with the shortcut key
    [MenuItem("Tools/Clear PlayerPrefs %#1")]
    private static void NewMenuOption()
    {
        PlayerPrefs.DeleteAll();
    }
    
    //Create a new submenu in the tools menu with options as an option
    [MenuItem("Tools/SubMenu/Options %2")]
    private static void NewNestedOption()
    {
        
    }
    
    [MenuItem("Assets/ProcessTexture")]
    private static void DoSomethingWithTexture()
    {
        
    }
    
    [MenuItem("Assets/ProcessTexture", true )]
    private static bool NewMenuOptionValidation()
    {
        return Selection.activeObject is Texture2D;
    }
    
    //ask cam about it
    [MenuItem("CONTEXT/RigidBody/New Option")]
    private static void NewMenuOption(MenuCommand menuCommand)
    {
        // The RigidBody component can be extracted from the menu command using the context field.
        var rigid = menuCommand.context as Rigidbody;
    }
}
