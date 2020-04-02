using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AnthonyY
{
    public class AnthonyStates : MonoBehaviour
    {
        public Animator animator;
        private JumpState jumpState;
        public Health health;
        // Start is called before the first frame update
        void Start()
        {
            health.deathEvent += HealthOndeathEvent;
        }

        private void HealthOndeathEvent(Health obj)
        {
            animator.SetTrigger("Player Death");
        }
        
        // Update is called once per frame
        private void OnTriggerEnter(Collider other)
        {
            if(other.CompareTag("Cube"))
            {
                animator.SetTrigger("Move player");
            }
            
            animator.SetTrigger("Player Jump");
            
        }

        private void OnCollisionEnter(Collision other)
        {
            animator.SetBool("isGrounded",true);
        }

        private void OnCollisionExit(Collision other)
        {
            animator.SetBool("isGrounded", false);
        }

        private void OnTriggerExit(Collider other)
        {
            animator.SetTrigger("I escaped");
            
        }
    }
}

