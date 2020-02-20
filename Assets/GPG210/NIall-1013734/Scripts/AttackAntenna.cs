using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackAntenna : MonoBehaviour
{
    private Transform t;
    public float distance;

    void Start()
    {
        t = GetComponent<Transform>();
    }

    public int damage;
    
    void FixedUpdate()
    {
        RaycastHit hit;
        if (Physics.Raycast(t.position, t.forward, out hit, distance))
        {
            if (hit.transform.GetComponent<Health>() != null)
            {
                // Do damage
                hit.transform.GetComponent<Health>().ChangeHealth(-damage);

                Debug.Log(gameObject.name + hit + "Damaged Unit");
                
                GetComponent<KillAmountScript>().IncreaseKills();
            }
        }
        
    }
}
