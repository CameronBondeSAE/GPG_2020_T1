using UnityEngine;

public class AntennaRC : MonoBehaviour
{
    private Transform t;
    public float distance;
    public float turnSpeed;
    public Rigidbody rb;

    void Start()
    {
        t = GetComponent<Transform>();
        rb = GetComponent<Rigidbody>();
    }


    void FixedUpdate()
    {
        if (Physics.Raycast(t.position, t.forward, distance))

        {
            rb.AddTorque(0, turnSpeed, 0);

            {
                // Debug.Log(gameObject.name + "Detected a wall");
                Debug.DrawRay(t.position, t.forward, Color.blue);
                rb.AddTorque(0, turnSpeed, 0);
            }
        }
    }
}