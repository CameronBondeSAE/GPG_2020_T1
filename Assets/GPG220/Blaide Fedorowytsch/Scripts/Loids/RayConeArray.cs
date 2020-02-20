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
        public bool addCentreRay;
        public float rayLength;
        public RaycastHit[] coneHit;
        public Ray[] coneRay;
        public Ray centreRay;
        public RaycastHit centreHit;
        
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
                if (Physics.Raycast(coneRay[r], out coneHit[r], rayLength))
                {
                    hitThisUpdate = true;
                }
            }

            if (Physics.Raycast(centreRay, out centreHit, rayLength))
            {
                hitThisUpdate = true;
            }

            if (hitThisUpdate == true)
            {
                RayConeArrayHitData rayConeArrayHitData = new RayConeArrayHitData(coneRay,coneHit,endCentre,centreHit,centreRay);
                rayConeArrayHit.Invoke(rayConeArrayHitData);
            }
        }

        private void UpdateRayArrayStructure()
        {
            centreRay = new Ray(transform.position,transform.forward);
            centreHit = new RaycastHit();

            coneRay = new Ray[resolution];
            coneHit = new RaycastHit[resolution];

            endCentre = transform.forward * rayLength;
            List<Vector3> localEndpoints = ConstructPolygon(resolution, rayEndRadius, endCentre, transform.rotation);
            // create a circle at the end of line from centre to rayend along transform forward.
            
            for (int r = 0; r < resolution; r++)
            {
                coneRay[r].direction = localEndpoints[r];
                coneRay[r].origin = transform.position + (coneRay[r].direction*rayStartlength);
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
            if (coneHit != null && coneRay != null)
            {
                for (int r = 0; r < coneRay.Length ; r++)
                {
                    Gizmos.color = (coneHit[r].collider == null) ? Color.white : Color.red;

                    if (coneHit[r].collider != null)
                    {
                        Gizmos.DrawLine(coneRay[r].origin,coneHit[r].point);  
                        Gizmos.DrawSphere(coneHit[r].point,0.1f);

                    }
                    else
                    {
                        Gizmos.DrawRay(coneRay[r].origin,coneRay[r].direction * rayLength);
                        Gizmos.DrawSphere(coneRay[r].GetPoint(rayLength), 0.1f);

                    }

                }
            }

            if (true)
            {
                Gizmos.color = (centreHit.collider == null) ? Color.white : Color.red;
                if (centreHit.collider != null)
                {
                    Gizmos.DrawRay(centreRay.origin,centreRay.direction *rayLength);
                }
            }
        }
    }

    public class RayConeArrayHitData
    {
        public Ray[] coneRay;
        public RaycastHit[] coneHit;
        public Ray centreRay;
        public RaycastHit centreHit;
        public Vector3 endCenter;
        public RayConeArrayHitData(Ray[]_coneRay,RaycastHit[]_coneHit,Vector3 _endCenter,RaycastHit _centreHit, Ray _centreRay)
        {
            coneRay = _coneRay;
            coneHit = _coneHit;
            endCenter = _endCenter;
            centreHit = _centreHit;
            centreRay = _centreRay;

        }
    }

}
