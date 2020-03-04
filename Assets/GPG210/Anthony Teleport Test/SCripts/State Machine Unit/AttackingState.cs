using System.Collections;
using System.Collections.Generic;
using AnthonyY;
using UnityEngine;

namespace AnthonyY
{
    public class AttackingState : StateBase
    {
        // Start is called before the first frame update
        public int damage = 100;
        private Rigidbody rb;
        public GameObject blood;
        private Transform currentTarget; //find target to explode
        void Start()
        {
            rb = GetComponent<Rigidbody>();
            GetComponent<Health>().deathEvent += Death;
        }

        // Update is called once per frame
        void Update()
        { 
            void OnCollisionEnter(Collision other)
            {
                // Does the other object even have a Health component?
                if (other.gameObject.GetComponent<Health>() != null)
                {
                    // Do damage
                    other.gameObject.GetComponent<Health>().ChangeHealth(-damage);
                }
            }

            
            
        }
        public void Death(Health health)
        {
            Instantiate(blood, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}

