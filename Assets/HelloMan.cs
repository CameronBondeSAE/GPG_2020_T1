﻿using System.Collections;
using System.Collections.Generic;
using GPG220.Luca.Scripts.Unit;
using UnityEngine;
using Mirror;

public class HelloMan : UnitBase
{

    [SyncVar] private Vector3 position;
    
    // Start is called before the first frame update
    
           
       
    

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
        
    }
    
    
}
