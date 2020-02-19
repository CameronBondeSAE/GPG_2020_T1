using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCRC : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        Physics.Raycast(transform.position, Vector3.forward, 10f);
    }
}
