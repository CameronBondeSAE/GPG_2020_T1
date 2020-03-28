using UnityEditor;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace CustomEditorDemo
{
    [CustomEditor(typeof(Tank))]
    public class LauncherEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            Tank tank = (Tank) target;
            if (GUILayout.Button("Fire"))
            {
                tank.Fire();
            }
        }

        [DrawGizmo(GizmoType.Pickable | GizmoType.Selected)]
        static void DrawGizmosSelected(Tank tank, GizmoType gizmoType)
        {
            var offsetPosition = tank.transform.TransformPoint(tank.offset);
            Handles.DrawDottedLine(tank.transform.position, offsetPosition, 3);
            Handles.Label(offsetPosition, "Offset");
            if (tank.projectile != null)
            {
                var positions = new List<Vector3>();
                var velocity = tank.transform.forward * tank.velocity / tank.projectile.mass;
                var position = offsetPosition;
                var physicsStep = 0.1f;
                //change float to determine distance of line
                for (var i = 0f; i <= 1f; i += physicsStep)
                {
                    positions.Add(position);
                    position += velocity * physicsStep;
                    velocity += Physics.gravity * physicsStep;
                }
                using (new Handles.DrawingScope(Color.red))
                {
                    Handles.DrawAAPolyLine(positions.ToArray());
                    Gizmos.DrawWireSphere(positions[positions.Count - 1], 0.125f);
                    Handles.Label(positions[positions.Count - 1], "Estimated Position (1 sec)");
                }
            }
        }

        void OnSceneGUI()
        {
            var launcher = target as Tank;
            var transform = launcher.transform;

            using (var cc = new EditorGUI.ChangeCheckScope())
            {
                var newOffset = transform.InverseTransformPoint(Handles.PositionHandle(transform.TransformPoint(launcher.offset), transform.rotation));

                if (cc.changed)
                {
                    Undo.RecordObject(launcher, "Offset Change");
                    launcher.offset = newOffset;
                }
            }

            Handles.BeginGUI();
            var rectMin = Camera.current.WorldToScreenPoint(launcher.transform.position + launcher.offset);
            var rect = new Rect();
            rect.xMin = rectMin.x;
            rect.yMin = SceneView.currentDrawingSceneView.position.height - rectMin.y;
            rect.width = 64;
            rect.height = 18;
            GUILayout.BeginArea(rect);
            using (new EditorGUI.DisabledGroupScope(!Application.isPlaying))
            {
                if (GUILayout.Button("Fire"))
                    launcher.Fire();
            }

            GUILayout.EndArea();
            Handles.EndGUI();
            
            
        }
    }
}