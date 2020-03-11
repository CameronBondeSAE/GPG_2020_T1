using System;
using System.Collections;
using System.Collections.Generic;
using AnthonyY;
using GPG220.Luca.Scripts.Abilities;
using UnityEngine;

namespace AnthonyY
{
    public class AttackingState : AbilityBase
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
            
            NewUnit = GetComponent<GameObject>();
        }
        
        public override bool Execute(GameObject executorGameObject, GameObject[] targets = null)
        {
            rb.AddForce(0,0,force,ForceMode.Impulse);
            //rb.transform.localScale += new Vector3(1,4,0); //rescale on runtime
            return true;
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
        
        
    }
}

