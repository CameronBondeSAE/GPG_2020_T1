using System;
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
        public GameObject NewUnit;
        public float force;
        public GameObject blood;
       
        
        
        void Start()
        {
            rb = GetComponent<Rigidbody>();
            GetComponent<Health>().deathEvent += Death;
            NewUnit = GetComponent<GameObject>();
        }

        public override void Update()
        {
            
            rb.AddForce(0,0,force,ForceMode.Impulse);
            //rb.transform.localScale += new Vector3(1,4,0); //rescale on runtime
        }

        


        void OnCollisionEnter(Collision other)
        {
            // Does the other object even have a Health component?
            if (other.gameObject.GetComponent<Health>() != null)
            {
                // Do damage
                other.gameObject.GetComponent<Health>().ChangeHealth(-damage);
            }
        }
        
        public void Death(Health health)
        {
            Instantiate(blood, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}

