using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace GPG220.Blaide_Fedorowytsch.Scripts.Loids
{
    public class RayConeArray : MonoBehaviour
    {
        public float rayEndRadius = 3f;
        public float rayStartlength;
        public int resolution;
        public float rayLength;
        public RaycastHit[] hit;
        public Ray[] ray;
        private Vector3 endCentre;
        [System.Serializable] public class RayConeArrayHit: UnityEvent<RayConeArrayHitData> {}
        [SerializeField] RayConeArrayHit rayConeArrayHit;
        // Start is called before the first frame update
        void Start()
        {
        
        }
        // Update is called once per frame
        void Update()
        {
            bool hitThisUpdate = false;
            UpdateRayArrayStructure();
            for (int r = 0; r < resolution; r++)
            {
                if (Physics.Raycast(ray[r], out hit[r], rayLength))
                {
                    hitThisUpdate = true;
                    
                }
            }
            if (hitThisUpdate == true)
            {
                RayConeArrayHitData rayConeArrayHitData = new RayConeArrayHitData(ray,hit,endCentre);
                rayConeArrayHit.Invoke(rayConeArrayHitData);
            }
        }

        private void UpdateRayArrayStructure()
        {

            ray = new Ray[resolution];
            hit = new RaycastHit[resolution];

            endCentre = transform.forward * rayLength;
            List<Vector3> localEndpoints = ConstructPolygon(resolution, rayEndRadius, endCentre, transform.rotation);
            // create a circle at the end of line from centre to rayend along transform forward.
            
            for (int r = 0; r < resolution; r++)
            {
                ray[r].direction = localEndpoints[r];
                ray[r].origin = transform.position + (ray[r].direction*rayStartlength);
            }
        }
        
        List<Vector3> ConstructPolygon(int vertexNumber, float radius, Vector3 centerPos,Quaternion rotation)
        {
            List<Vector3> points = new List<Vector3>();
            float angle = 2 * Mathf.PI / vertexNumber;

            for (int i = 0; i < vertexNumber; i++)
            {
                Matrix4x4 rotationMatrix = new Matrix4x4(new Vector4(Mathf.Cos(angle * i), Mathf.Sin(angle * i), 0, 0),
                    new Vector4(-1 * Mathf.Sin(angle * i), Mathf.Cos(angle * i), 0, 0),
                    new Vector4(0, 0, 1, 0),
                    new Vector4(0, 0, 0, 1));
                Vector3 initialRelativePosition = new Vector3(0, radius, 0);
                //lineRenderer.SetPosition(i, centerPos + rotationMatrix.MultiplyPoint(initialRelativePosition));
                Vector3 point = centerPos + rotationMatrix.MultiplyPoint(initialRelativePosition);
                point = rotation * (point - centerPos) + centerPos;
                points.Add(point);
                
              
            }
            return points;
        }
        
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.blue;
            
            Gizmos.DrawLine(transform.position, transform.position + transform.forward * rayLength);
            Gizmos.DrawSphere(transform.position + transform.forward * rayLength,0.1f);

            if (hit != null && ray != null)
            {
                for (int r = 0; r < ray.Length ; r++)
                {
                    Gizmos.color = (hit[r].collider == null) ? Color.white : Color.red;

                    if (hit[r].collider != null)
                    {
                        Gizmos.DrawLine(ray[r].origin,hit[r].point);  
                        Gizmos.DrawSphere(hit[r].point,0.1f);

                    }
                    else
                    {
                        Gizmos.DrawRay(ray[r].origin,ray[r].direction * rayLength);
                        Gizmos.DrawSphere(ray[r].GetPoint(rayLength), 0.1f);

                    }

                }
            }
        }
    }

    public class RayConeArrayHitData
    {
        public Ray[] ray;
        public RaycastHit[] hit;
        public Vector3 endCenter;
        public RayConeArrayHitData(Ray[]_ray,RaycastHit[]_hit,Vector3 _endCenter)
        {
            ray = _ray;
            hit = _hit;
            endCenter = _endCenter;
        }
    }

}
