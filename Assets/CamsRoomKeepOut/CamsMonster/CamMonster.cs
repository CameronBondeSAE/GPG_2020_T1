using System;
using System.Collections;
using System.Collections.Generic;
using GPG220.Luca.Scripts.Unit;
using UnityEngine;

public class CamMonster : UnitBase
{
    private List<UnitBase> UnitBases;
    
    // Start is called before the first frame update
    void Start()
    {
        Initialize();
    
        // Subscribe to event
        GetComponent<Health>().deathEvent += Death;
    }

    // private void OnTriggerEnter(Collider other)
    // {
        // UnitBases.Add(other.GetComponent<UnitBase>());
    // }

    public void Death(Health health1)
    {
        
        
        // foreach (UnitBase item in UnitBases)
        // {
        //     if (item is CamMonster)
        //     {
        //         // Using 'as' with a temp variable
        //         CamMonster camMonster = item as CamMonster;
        //         camMonster.Breakdance();
        //         
        //         // Using brackets to scope the whole cast
        //         (item as CamMonster).Breakdance();
        //         
        //         // Older style bracket casting
        //         ((CamMonster)item).Breakdance();
        //     }
        // }
        
        Destroy(gameObject);
    }

    public void Breakdance()
    {
        Debug.Log("WOOOO!");
    }
}
