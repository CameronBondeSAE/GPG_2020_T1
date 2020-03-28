using Pathfinding.Boids;
using Pathfinding.Boids.Behaviour_Scripts;
using UnityEditor;
using UnityEngine;

namespace BoidsEditor
{
    [CustomEditor(typeof(CompositeBehaviour))]
    public class CompositeBehaviourEditor : Editor
    {
        /// <summary>
        /// Editor stuff for each array field and allows for array changes 
        /// </summary>
        public override void OnInspectorGUI()
        {
            CompositeBehaviour cb = (CompositeBehaviour) target;

            if (cb.behaviours == null || cb.behaviours.Length == 0)
            {
                if (GUILayout.Button("Add Behaviour"))
                {
                    //Add behaviour
                    AddBehaviour(cb);
                    //ensures changes are saved
                    EditorUtility.SetDirty(cb);
                }
            }
            else
            {
                //sets up the fields for each behaviour
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Number", GUILayout.MinWidth(60f), GUILayout.MaxWidth(60f));
                EditorGUILayout.LabelField("Behaviors", GUILayout.MinWidth(60f));
                EditorGUILayout.LabelField("Weights", GUILayout.MinWidth(60f), GUILayout.MaxWidth(60f));
                EditorGUILayout.EndHorizontal();

                //check for changes
                EditorGUI.BeginChangeCheck();

                //sets up the visuals for each array 
                for (int i = 0; i < cb.behaviours.Length; i++)
                {
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField(i.ToString(), GUILayout.MinWidth(60f), GUILayout.MaxWidth(60f));
                    cb.behaviours[i] = (FlockBehaviour) EditorGUILayout.ObjectField(cb.behaviours[i],
                        typeof(FlockBehaviour), false, GUILayout.MinWidth(60f));
                    cb.weights[i] =
                        EditorGUILayout.FloatField(cb.weights[i], GUILayout.MinWidth(60f), GUILayout.MaxWidth(60f));
                    EditorGUILayout.EndHorizontal();
                }

                if (EditorGUI.EndChangeCheck())
                {
                    EditorUtility.SetDirty(cb);
                }

                EditorGUILayout.EndHorizontal();


                //adding move behaviours
                EditorGUILayout.BeginHorizontal();
                if (GUILayout.Button("Add Behaviour"))
                {
                    //Add behaviour
                    AddBehaviour(cb);
                    //ensures changes are saved
                    EditorUtility.SetDirty(cb);
                }


                //removes behaviour
                if (cb.behaviours != null || cb.behaviours.Length > 0)
                {
                    if (GUILayout.Button("Remove Behaviour"))
                    {
                        //Remove Behaviour
                        RemoveBehaviour(cb);
                        //ensures changes are saved
                        EditorUtility.SetDirty(cb);
                    }
                }
            }
        }

        void AddBehaviour(CompositeBehaviour cb)
        {
            int oldCount = (cb.behaviours != null) ? cb.behaviours.Length : 0;
            FlockBehaviour[] newBehaviours = new FlockBehaviour[oldCount + 1];
            float[] newWeights = new float[oldCount + 1];
            for (int i = 0; i < oldCount; i++)
            {
                newBehaviours[i] = cb.behaviours[i];
                newWeights[i] = cb.weights[i];
            }

            newWeights[oldCount] = 1f;
            cb.behaviours = newBehaviours;
            cb.weights = newWeights;
        }

        void RemoveBehaviour(CompositeBehaviour cb)
        {
            int oldCount = cb.behaviours.Length;
            if (oldCount == 1)
            {
                cb.behaviours = null;
                cb.weights = null;
                return;
            }

            FlockBehaviour[] newBehaviours = new FlockBehaviour[oldCount - 1];
            float[] newWeights = new float[oldCount - 1];
            for (int i = 0; i < oldCount - 1; i++)
            {
                newBehaviours[i] = cb.behaviours[i];
                newWeights[i] = cb.weights[i];
            }

            cb.behaviours = newBehaviours;
            cb.weights = newWeights;
        }
    }
}