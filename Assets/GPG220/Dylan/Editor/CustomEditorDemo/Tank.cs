using UnityEngine;

namespace CustomEditorDemo
{
    public class Tank : MonoBehaviour
    {
        public Rigidbody projectile;
        public Vector3 offset = Vector3.forward;

        [Range(0, 100)]
        public float velocity = 10;

        [ContextMenu("Fire")]
        public void Fire()
        {
            var body = Instantiate(projectile, transform.TransformPoint(offset), transform.rotation);
            body.velocity = transform.TransformDirection(Vector3.forward * velocity);
        }

        public float rotateSpeed = 10;
        public float moveSpeed = 10;
        public void Update()
        {
        
            float rotateY = Input.GetAxisRaw("Horizontal") * rotateSpeed;
        
            float moveX = Input.GetAxisRaw("Vertical") * moveSpeed;
        
            transform.Rotate(0,rotateY,0); 
            transform.Translate(new Vector3(0, 0 ,moveX ));

            if (Input.GetKeyDown(KeyCode.Space))
            {
                Fire();
            }
        
            if (Input.GetKey(KeyCode.P))
            {
                velocity += 1f;
            }
            if (Input.GetKey(KeyCode.O))
            {
                velocity -= 1f;
            }
        }
    }
}