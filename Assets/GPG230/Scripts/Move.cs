using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Stephen
{



    public class Move : MonoBehaviour
    {
        private Rigidbody rb;
        
        // Start is called before the first frame update
        void Start()
        {
            rb = GetComponent<Rigidbody>();
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(("W")))
            {
                rb.AddRelativeForce(Vector3.forward);
            }
            if (Input.GetKeyDown(("s")))
            {
                rb.AddRelativeForce(Vector3.back);
            }
            if (Input.GetKeyDown(("a")))
            {
                rb.AddRelativeForce(Vector3.left);
            }
            if (Input.GetKeyDown(("d")))
            {
                rb.AddRelativeForce(Vector3.right);
            }
        }
    }
    
    
    
    
}