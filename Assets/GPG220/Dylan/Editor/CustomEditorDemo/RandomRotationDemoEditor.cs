using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


[CustomEditor(typeof(RandomRotationDemo))]
public class RandomRotationDemoEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        RandomRotationDemo myScript = (RandomRotationDemo) target;

        //myScript.prefab = EditorGUILayout.ObjectField(myScript.prefab);
        if (GUILayout.Button("Rotate"))
        {
            myScript.RandomRotation();
        }
    }
    
    
}
