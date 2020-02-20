using System;
using UnityEngine;

namespace GPG220.Blaide_Fedorowytsch.Scripts.Loids.Steering_Behaviours
{
    public class Avoidance : SteeringBehaviourBase
    {
        public float forceMultiplier;
        public override void Start()
        {
            base.Start();
            GetComponent<RayConeArray>().RayConeArrayHitEvent += OnRayHit;
        }



        // Avoiding 
        private void OnRayHit(RayConeArrayHitData rayConeArrayHitData)
        {
            for (int r = 0; r < rayConeArrayHitData.coneRay.Length; r++)
            {
                if (rayConeArrayHitData.coneHit[r].collider != null)
                {
                    RaycastHit hit = rayConeArrayHitData.coneHit[r];
                   Ray centreLine = new Ray(transform.position,transform.forward);
                   rB.AddForceAtPosition(((centreLine.GetPoint(hit.distance) - hit.point )* 1/hit.distance) * forceMultiplier /rayConeArrayHitData.coneRay.Length,hit.point);
                }
            }
            if (rayConeArrayHitData.centreHit.collider != null)
            {
                rB.AddForce(-((transform.forward) * forceMultiplier*10 * 15/rayConeArrayHitData.centreHit.distance));
                rB.AddTorque(transform.right * forceMultiplier*10 * 2/rayConeArrayHitData.centreHit.distance);
            }
        }
    }
}
