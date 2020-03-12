using System.Collections;
using System.Collections.Generic;
using GPG220.Blaide_Fedorowytsch.Scripts;
using Mirror.Examples.Chat;
using UnityEngine;

public class TriggerPoint : MonoBehaviour
{
    public float radius;

    public GameObject sphere;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, radius);
        foreach (Collider PlayerObj in colliders)
        {
            if (PlayerObj.gameObject.CompareTag("Enemy"))
            {
                Debug.Log("Attacking Now!");
            }
            else
            {
                Debug.Log("Im not Attacking");
            }
        }
    }
}