using System;
using System.Collections;
using System.Collections.Generic;
using GPG220.Luca.Scripts.Unit;
using UnityEditor;
using UnityEngine;

public class UnitKiller : EditorWindow
{
   // private List<UnitBase> units;
    
    [MenuItem("Window/UnitKiller")]
    public static void ShowWindow()
    {
        GetWindow<UnitKiller>();
    }

    private void OnGUI()
    {
        GUILayout.Label("Basic unit Utilities",EditorStyles.largeLabel);

        if (GUILayout.Button("Kill All Units"))
        {
            List<UnitBase> units;
            foreach (UnitBase unit in FindObjectsOfType<UnitBase>())
            {
                Health health = unit.gameObject.GetComponent<Health>();
                if (health != null)
                {
                    health.ChangeHealth(-999);
                }
            }

        }
    }
    
}
