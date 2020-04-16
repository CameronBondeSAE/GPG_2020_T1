using System;
using System.Collections;
using System.Collections.Generic;
using System.Timers;
using UnityEngine;

public class GotHitEffectTest : MonoBehaviour
{
    private Health health;
    
    private MeshRenderer meshRenderer;
    public float maxAmount;
    public float minAmount;
    public float speed;
   
    
    // Start is called before the first frame update
    void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();

        health = GetComponent<Health>();

        health.healthChangedEvent += WibblyWobble;
        
        
    }

    // Update is called once per frame
    void Update()
    {
        if (meshRenderer.material.GetFloat("_Amount") > minAmount)
        {
            
            meshRenderer.material.SetFloat("_Amount", meshRenderer.material.GetFloat("_Amount") - Time.deltaTime*speed);
        }

        if (Input.GetKeyDown("space"))
        {
            WibblyWobble(health,50);
        }
    }

    public void WibblyWobble(Health health, int amount)
    {
        
        float percent = amount / (float)health.CurrentHealth * 100;
        
        
        meshRenderer.material.SetFloat("_Amount",amount/100);
        
    }

    private void OnDestroy()
    {
        health.healthChangedEvent -= WibblyWobble;
    }
}
