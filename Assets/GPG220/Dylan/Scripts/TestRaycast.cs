using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestRaycast : MonoBehaviour
{
    public static TestRaycast instance = null;
    public Camera camera;
    public LayerMask testMask;
    
    public void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != null)
        {
            Destroy(gameObject);
        }
    }

    //to test for raycast while in console
    public RaycastHit ShootRaycast(LayerMask toDestroy)
    {

        RaycastHit hitInfo;
        Ray ray = camera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hitInfo, Mathf.Infinity, toDestroy))
        {
            Debug.Log(hitInfo.collider.name);
            return hitInfo;
        }
        
        
        return hitInfo;
    }
}
