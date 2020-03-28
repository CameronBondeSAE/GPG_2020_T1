using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class DemoEditorWindow : EditorWindow
{
    private string myString = "Hello World";
    
    //determines the window menu name
    [MenuItem("Window/Thing")]
    public static void ShowWindow()
    {
        GetWindow<DemoEditorWindow>("Example");
    }

    private void OnGUI()
    {
        GUILayout.Label("This is a label", EditorStyles.boldLabel);

        myString = EditorGUILayout.TextField("Name", myString);

        //put function button calls in here
        if (GUILayout.Button("Press me"))
        {
            
        }
    }
}
