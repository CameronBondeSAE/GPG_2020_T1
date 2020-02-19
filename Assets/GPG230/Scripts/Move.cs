using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
namespace Stephen
{



    public class Move : NetworkBehaviour
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
            if (isLocalPlayer)
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
    
    
    
    
}