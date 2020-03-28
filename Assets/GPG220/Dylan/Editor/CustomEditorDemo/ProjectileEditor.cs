using UnityEditor;
using UnityEngine;

namespace CustomEditorDemo
{
    [CustomEditor(typeof(Projectile))]
    public class ProjectileEditor : Editor
    {
    
        [DrawGizmo(GizmoType.Selected | GizmoType.NonSelected)]
        static void DrawGizmosSelected(Projectile projectile, GizmoType gizmoType)
        {
            Gizmos.DrawSphere(projectile.transform.position, 0.125f);
        }
    
        //on scene GUi is called when the scene view is
        ////rendered allowing for widget drawing inside scene view
        void OnSceneGUI()
        {
            var projectile = target as Projectile;
            var transform = projectile.transform;
            projectile.damageRadius = Handles.RadiusHandle(transform.rotation, transform.position, projectile.damageRadius);
        }
    
    }
}
