using System;
using UnityEngine;

namespace GPG220.Luca.Scripts.Pathfinding
{
    public class Obstacle : MonoBehaviour
    {
        public float forceMultiplier = 2;
        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
        
        }

        private void OnCollisionEnter(Collision other)
        {
            Rigidbody rb = other.collider.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.AddForce(-other.impulse * forceMultiplier, ForceMode.Impulse);
            }
        }
    }
}
