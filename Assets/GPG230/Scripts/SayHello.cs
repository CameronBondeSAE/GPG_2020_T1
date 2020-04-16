using System.Collections;
using System.Collections.Generic;
using GPG220.Luca.Scripts.Abilities;
using UnityEngine;

public class SayHello : AbilityBase
{

    private Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("space"))
        {
            SelectedExecute();
            
        }   
    }

    public override bool Execute(GameObject executorGameObject, GameObject targets = null)
    {
        throw new System.NotImplementedException();
    }

    public override bool SelectedExecute()
    {
        Debug.Log("Hello");

        rb.AddForce((Vector3.up)* 10000);

        NotifyAbilityStartExecution(gameObject);
       
        return base.SelectedExecute();
        
        
    }
}
