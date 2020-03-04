using System;
using System.Collections;
using System.Collections.Generic;
using AnthonyY;
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

        // Update is called once per frame
        void Update()
        {
            transform.Translate(Vector3.forward * Time.deltaTime);
            rb.constraints = RigidbodyConstraints.FreezePositionY;

        }
    }

}
