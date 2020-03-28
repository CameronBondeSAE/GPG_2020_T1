using System;
using UnityEngine;

namespace LineOfSight.Scripts
{
    [RequireComponent(typeof(Rigidbody))]
    public class Controller : MonoBehaviour
    {
        public float moveSpeed = 6;
        
        private Rigidbody rb;
        private Camera viewCamera;
        private Vector3 velocity;
        
        void Start()
        {
            rb = GetComponent<Rigidbody>();
            viewCamera = Camera.main;
        }

        // Update is called once per frame
        void Update()
        {
            Vector3 mousePos = viewCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, viewCamera.transform.position.y));
            transform.LookAt(mousePos + Vector3.up * transform.position.y);
            velocity = new Vector3(Input.GetAxisRaw("Horizontal"),0,Input.GetAxisRaw("Vertical")).normalized * moveSpeed;
        }

        private void FixedUpdate()
        {
           rb.MovePosition(rb.position + velocity * Time.fixedDeltaTime); 
        }
    }
}
