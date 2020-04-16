using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GotHitEffectTest : MonoBehaviour
{
    private MeshRenderer meshRenderer;
    public float Amount;

    public Color mycolor;
    
    // Start is called before the first frame update
    void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        
        
        
    }

    // Update is called once per frame
    void Update()
    {
        
        meshRenderer.material.SetFloat("_Amount", Amount);
        
    }
}
