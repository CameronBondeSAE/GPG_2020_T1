using System;
using GPG220.Luca.Scripts.Unit;
using UnityEngine;

namespace GPG220.Luca.Scripts.Pathfinding
{
    public class Obstacle : MonoBehaviour
    {
        public float forceMultiplier = 2;

        public float pushCooldown = 2;

        public float pushCooldownCounter = 0;
        
        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
            if (pushCooldownCounter > 0)
                pushCooldownCounter -= Time.deltaTime;
        }

        private void OnCollisionEnter(Collision other)
        {
            if (other.rigidbody != null)
            {
                UnitBase unitBase = other.collider.GetComponent<UnitBase>();
                if (unitBase == null)
                    return;
                
                var otherYBottom = other.collider.transform.position.y - (other.collider.bounds.extents.y * .9f);
                
                if(other.contactCount > 0 && other.contacts[0].point.y > otherYBottom)
                    other.rigidbody.AddForce(-other.impulse * forceMultiplier, ForceMode.Impulse);
                
                /*// Hack; 
                var impulseZeroDir = other.impulse;
                impulseZeroDir.y = 0;
                var angle = Vector3.Angle(other.impulse, impulseZeroDir);
                if(angle < 40)
                    other.rigidbody.AddForce(-other.impulse * forceMultiplier, ForceMode.Force);*/
            }
        }

        private void OnCollisionStay(Collision other)
        {
            if (other.rigidbody != null && pushCooldownCounter <= 0)
            {
                UnitBase unitBase = other.collider.GetComponent<UnitBase>();
                if (unitBase == null)
                    return;

                var otherYBottom = other.collider.transform.position.y - (other.collider.bounds.extents.y * .9f);
                var dir = other.collider.transform.position - other.contacts[0].point;//transform.position;
                dir.y = 0; // TODO HACK
                if (other.contactCount > 0 && other.contacts[0].point.y > otherYBottom)
                {
                    //Debug.Log("PUSH!");
                    other.rigidbody.AddForce(dir.normalized * forceMultiplier, ForceMode.Impulse);
                }
                
                /*var dir = other.collider.transform.position - transform.position;
                var zeroYDir = dir;
                dir.y = 0;
                
                // Inperformant Hack; 
                var angle = Vector3.Angle(dir, zeroYDir);
                if(angle < 40)
                    other.rigidbody.AddForce(dir.normalized * forceMultiplier, ForceMode.Force);*/

                pushCooldownCounter = pushCooldown;
            }
        }
    }
}
