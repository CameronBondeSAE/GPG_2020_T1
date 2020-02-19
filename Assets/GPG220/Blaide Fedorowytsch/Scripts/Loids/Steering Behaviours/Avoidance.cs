using System;
using UnityEngine;

namespace GPG220.Blaide_Fedorowytsch.Scripts.Loids.Steering_Behaviours
{
    public class Avoidance : MonoBehaviour
    {
        private Rigidbody rB;
        public float forceMultiplier;
        private void Start()
        {
            rB = GetComponent<Rigidbody>();
        }


        // Avoiding 
        public void OnRayHit(RayConeArrayHitData rayConeArrayHitData)
        {
            for (int r = 0; r < rayConeArrayHitData.ray.Length; r++)
            {
                if (rayConeArrayHitData.hit[r].collider != null)
                {


                   RaycastHit hit = rayConeArrayHitData.hit[r];
                   Ray centreLine = new Ray(transform.position,transform.forward);
                   rB.AddForceAtPosition(((centreLine.GetPoint(hit.distance) - hit.point )* 1/hit.distance) * forceMultiplier /rayConeArrayHitData.ray.Length,hit.point);
                   
                }
            }
            
        }

    }
}
