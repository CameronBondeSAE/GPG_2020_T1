using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject thing;
    
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < 10; i++)
        {
            Instantiate(thing, Vector3.zero, Quaternion.identity);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
