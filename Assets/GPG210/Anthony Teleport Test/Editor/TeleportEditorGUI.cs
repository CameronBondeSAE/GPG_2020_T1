using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
[CustomEditor(typeof(TeleportEditor))]
public class TeleportEditorGUI : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        TeleportEditor myScript = (TeleportEditor) target;

        if (GUILayout.Button("Teleport"))
        {
            myScript.Teleporting();
           
        }

        if (GUILayout.Button("Walk"))
        {
            myScript.StartWalk();
        }
            
    }

   
}
