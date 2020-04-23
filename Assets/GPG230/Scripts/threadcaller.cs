using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class threadcaller : MonoBehaviour
{

    public TheadyBoi TheadyBoi;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
        if (Input.GetKeyDown("w"))
        {
            TheadyBoi.Requestmaths(1,OnCallBack);
        }
        
    }

    public void OnCallBack(int number)
    {
        Debug.Log(number);
        
    }
    
    
    
}
