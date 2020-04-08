using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Cinemachine;
using UnityEngine;

public class TheadyBoi : MonoBehaviour
{

    int count = 0;
    public int speed;
    private Thread thread;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
        transform.Rotate(0,Time.deltaTime*speed,0);
        
        if (Input.GetKeyDown("s"))
        {
            thread = new Thread(DoWork);
            thread.Start();
        }
        
        
    }

    void DoWork()
    {

        

        for (int i = 0; i < 300000000; i++)
        {

            count++;

        }

        Debug.Log("Done");
    }
    
}
