using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GUYMODE1 : MonoBehaviour
{
    private Rigidbody rb;

    private float speed;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        rb.AddRelativeForce(0,0,speed);
    }
}
