using System.Diagnostics;
using UnityEditor;
using UnityEngine;
using Debug = UnityEngine.Debug;
using Grid = Pathfinding.AStar.New.Grid;

namespace AStar
{
    [CustomEditor(typeof(Grid))]
    public class GridEditorScript : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            Grid grid = (Grid) target;
            if (GUILayout.Button("ReInitialize"))
            {
                grid.InitializeWorld();
                Debug.Log("Initialized");
            }
        }
    }
}
