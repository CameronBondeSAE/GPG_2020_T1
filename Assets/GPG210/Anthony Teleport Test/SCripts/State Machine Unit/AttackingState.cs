using System;
using System.Collections;
using System.Collections.Generic;
using AnthonyY;
using GPG220.Luca.Scripts.Abilities;
using UnityEditorInternal;
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
            
            NewUnit = GetComponent<GameObject>();
        }
        public override void Enter()
        {
            base.Enter();
        }

        public override void Execute()
        {
            base.Execute();
            rb.AddForce(0,0,force,ForceMode.Impulse);
            //rb.transform.localScale += new Vector3(1,4,0); //rescale on runtime
            
        }

        public override void Exit()
        {
            base.Exit();
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

