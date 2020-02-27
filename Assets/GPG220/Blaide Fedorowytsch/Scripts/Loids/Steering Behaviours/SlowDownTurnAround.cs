using System;
using UnityEngine;

namespace GPG220.Blaide_Fedorowytsch.Scripts.Loids.Steering_Behaviours
{
    public class SlowDownTurnAround : SteeringBehaviourBase
    {
        public float forceMultiplier;
        public Vector3[] turnAroundDirections;
        public int turnAroundDirectionIndex;
        public bool CentreHitLast;
        public float centreHitTimeOut;
        public float centrehitTimer;

        public override void Start()
        {
            base.Start();
            GetComponent<RayConeArray>().rayConeArrayHitEvent += OnRayHit;
            turnAroundDirections = new Vector3[]
                {transform.TransformDirection(Vector3.right), transform.TransformDirection(-Vector3.right)};
        }

        public void FixedUpdate()
        {
            if (CentreHitLast)
            {
                centrehitTimer += Time.deltaTime;
                if (centrehitTimer >= centreHitTimeOut)
                {
                    CycleTurnDirection();
                    centrehitTimer = 0;
                }
            }

        }


        // Avoiding 
        private void OnRayHit(RayConeArrayHitData rayConeArrayHitData)
        {
            for (int r = 0; r < rayConeArrayHitData.coneRay.Length; r++)
            {
                if (rayConeArrayHitData.coneHit[r].collider != null)
                {
                    RaycastHit hit = rayConeArrayHitData.coneHit[r];
                    Ray centreLine = new Ray(transform.position, transform.forward);
                    rB.AddForceAtPosition(
                        ((centreLine.GetPoint(hit.distance) - hit.point) * 1 / hit.distance) * forceMultiplier /
                        rayConeArrayHitData.coneRay.Length, hit.point);
                }
            }

            // 
            if (rayConeArrayHitData.centreHit.collider != null)
            {
                rB.AddForce(-((transform.forward) * forceMultiplier * 10 * 15 /
                              rayConeArrayHitData.centreHit.distance));

                if (!CentreHitLast)
                {
                    CycleTurnDirection();
                }

                rB.AddTorque(turnAroundDirections[turnAroundDirectionIndex] * forceMultiplier * 10 * 2 /
                             rayConeArrayHitData.centreHit.distance);

                CentreHitLast = true;
            }
            else
            {
                CentreHitLast = false;
            }
        }

        void CycleTurnDirection()
        {
            if (turnAroundDirectionIndex >= turnAroundDirections.Length - 1)
            {
                turnAroundDirectionIndex = 0;
            }
            else
            {
                turnAroundDirectionIndex++;
            }
        }
    }
}
