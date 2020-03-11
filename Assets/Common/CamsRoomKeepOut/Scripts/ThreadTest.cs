using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class ThreadTest : MonoBehaviour
{
    static readonly object _lockObject = new object();

    long totalCount = 0;
    private float speed = 100;

    private void Update()
    {
        transform.Rotate(0, Time.deltaTime * speed, 0);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            new Thread(DoWork).Start();
        }
    }

    void DoWork()
    {
        Debug.Log("Start");

        int i = 0;
        long count = 0;

        for (i = 0; i < 1000000000; i++)
        {
            count++;
        }

        lock (_lockObject)
        {
            totalCount += count;
        }

        Debug.Log("Done = " + totalCount + " i = " + i);
    }
}