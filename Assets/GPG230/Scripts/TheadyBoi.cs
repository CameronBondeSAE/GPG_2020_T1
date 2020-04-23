using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using Cinemachine;
using UnityEngine;
using UnityEngine.UI;

public class TheadyBoi : MonoBehaviour
{

    int count = 0;
    public int speed;
    private Thread thread;

    public delegate void StephenCallBack(int SickMaths);
    
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

    public async void Requestmaths(int Number, StephenCallBack callBack)
    {


        int res = await Task.Run(() => DoMaths(Number));
        callBack(res);

    }


    public int DoMaths(int number)
    {
        Thread.Sleep(5000);
        
        return number+number;
    }
    
    

}
