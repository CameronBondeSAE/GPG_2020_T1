using System;
using System.Collections;
using System.Collections.Generic;
using AnthonyY;
using GPG220.Luca.Scripts.Abilities;
using UnityEngine;

namespace AnthonyY
{
    public class WalkingState : StateBase
    {
        public Rigidbody rb;
        void Start()
        {
            rb = GetComponent<Rigidbody>();
           
        }
        public override void Enter()
        {
            base.Enter();
        }

        public override void Execute()
        {
            base.Execute();
            transform.Translate(Vector3.forward * Time.deltaTime);
            rb.constraints = RigidbodyConstraints.FreezePositionY;
            
        }

        public override void Exit()
        {
            base.Exit();
        }

       
        
        


    }

}
