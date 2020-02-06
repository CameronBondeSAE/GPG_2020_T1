using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(TeleportUnit))]
public class TeleportUnitEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        
        TeleportUnit myScript = (TeleportUnit)target;
        
        if(GUILayout.Button("Walking"))
        {
            myScript.Walking();
        }
        
        if(GUILayout.Button("Sprinting"))
        {
            myScript.Sprinting();
        }
        
        if(GUILayout.Button("Teleporting"))
        {
            myScript.Teleporting();
        }
        
        if(GUILayout.Button("Attacking"))
        {
            myScript.Attacking();
        }
    }
}
