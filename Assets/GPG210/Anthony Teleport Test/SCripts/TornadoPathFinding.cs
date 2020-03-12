using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class TornadoPathFinding : MonoBehaviour
{
    public Transform[] target;

    public float speed;

    private int currentTarget;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position != target[currentTarget].position)
        {
            
            Vector3 pos = Vector3.MoveTowards(transform.position, target[currentTarget].position,
                speed * Time.deltaTime);
                GetComponent<Rigidbody>().MovePosition(pos);
                
        }
        else
        {
            currentTarget = (currentTarget + 1) % target.Length;
        }
    }
}
