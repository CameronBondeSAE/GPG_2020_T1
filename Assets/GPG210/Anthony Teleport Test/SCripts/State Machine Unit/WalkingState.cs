using System;
using System.Collections;
using System.Collections.Generic;
using AnthonyY;
using GPG220.Luca.Scripts.Abilities;
using UnityEngine;

namespace AnthonyY
{
    public class WalkingState : AbilityBase
    {
        
        public Rigidbody rb;
        
        void Start()
        {
            rb = GetComponent<Rigidbody>();
           
        }

        public override bool Execute(GameObject executorGameObject, GameObject[] targets = null)
        {
            transform.Translate(Vector3.forward * Time.deltaTime);
            rb.constraints = RigidbodyConstraints.FreezePositionY;
            return true;
        }
    }

}
