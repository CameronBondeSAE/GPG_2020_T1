using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Editorbutton))]
public class EditorButtonEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        
        Editorbutton myScript = (Editorbutton)target;
        if(GUILayout.Button("Attacking"))
        {
            myScript.Attack();
        }
        if(GUILayout.Button("Charging"))
        {
            myScript.Charging();
        }

    }
}
