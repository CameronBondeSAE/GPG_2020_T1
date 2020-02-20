using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
namespace Stephen
{



    public class Move : NetworkBehaviour
    {
        private Rigidbody rb;
        public float speed;

        [SyncVar] private Vector3 position;
        
        
        // Start is called before the first frame update
        void Awake()
        {
           
            rb = GetComponent<Rigidbody>();
        }

        // Update is called once per frame
        void Update()
        {
            if (isServer)
            {
                position = transform.position;

            }

            if (isClient)
            {
                transform.position = position;
            }
            
            if (isLocalPlayer)
            {
                if (Input.GetKeyDown(KeyCode.W))
                {
                    CmdJump();
                }
            }
            
            
        }

        [Command]
        public void CmdJump()
        {
            rb.AddForce(Vector3.up *speed);
            
        }
        
    }
    
    
    
    
}