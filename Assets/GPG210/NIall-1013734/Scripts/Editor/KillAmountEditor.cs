using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(KillAmountScript))]
public class KillAmountEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        KillAmountScript myScript = (KillAmountScript) target;
        if (GUILayout.Button("Increase Amount"))
        {
            myScript.IncreaseKills();
        }
        
        if (GUILayout.Button("Decrease Amount"))
        {
            myScript.DecreaseKills();
        }
    }
}
