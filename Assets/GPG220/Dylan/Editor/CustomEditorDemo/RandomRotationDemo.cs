using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class RandomRotationDemo : EditorWindow
{
    public GameObject prefab;

    public void RandomRotation()
    {
        prefab.transform.Rotate(0,Random.Range(0,360),0);
    }
    
    //determines the window menu name
    [MenuItem("Window/Rotate")]
    public static void ShowWindow()
    {
        GetWindow<DemoEditorWindow>("Example");
    }

    private void OnGUI()
    {
        GUILayout.Label("This is a label", EditorStyles.boldLabel);
        
        
                
        //put function button calls in here
        if (GUILayout.Button("Press me"))
        {
            RandomRotation();
        }
    }
}
