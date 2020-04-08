using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class Changecolour : NetworkBehaviour
{
    private float timer;

    [SyncVar] public Color lightcolour;
    // Start is called before the first frame update
    void Start()
    {
        
        GetComponent<Light>().color = lightcolour;
           
        timer = 1f;
            
    }

    // Update is called once per frame
    void Update()
    {
        
        if (isServer)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {

                lightcolour = Random.ColorHSV();
            }
        }

        if (isClient)
        {
            GetComponent<Light>().color = lightcolour;
        }

    }
    
    
    
}
