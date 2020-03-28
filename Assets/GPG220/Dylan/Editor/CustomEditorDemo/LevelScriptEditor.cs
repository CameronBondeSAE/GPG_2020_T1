using UnityEditor;
using UnityEngine;

namespace CustomEditorDemo
{
    [UnityEditor.CustomEditor(typeof(LevelScript))]
    public class LevelScriptEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            //change values out of runtime
            LevelScript myTarget = (LevelScript)target;

            myTarget.experience = EditorGUILayout.IntField("Experience", myTarget.experience);
            EditorGUILayout.LabelField("Level", myTarget.Level.ToString());
            

            //draw a small box that contains text/*
            
                     
            EditorGUILayout.HelpBox("I AM HERE TO HELP", MessageType.Info);
            
            
            //custom button to call functions

            LevelScript myScript = (LevelScript) target;
            if (GUILayout.Button("Build Object"))
            {
                myScript.BuildObject();
            }
        }
    }
}
