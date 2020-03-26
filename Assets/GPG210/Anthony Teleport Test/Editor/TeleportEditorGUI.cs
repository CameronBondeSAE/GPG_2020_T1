using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
[CustomEditor(typeof(Teleporter))]
public class TeleportEditorGUI : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        Teleporter myScript = (Teleporter) target;

        /*if (GUILayout.Button("Teleport"))
        {
            myScript.Teleporting();
           
        }*/

        
            
    }

   
}
