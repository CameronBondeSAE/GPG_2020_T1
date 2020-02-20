using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Spawner : MonoBehaviour
{
    public GameObject yourPrefab;
    public int xAmount;
    public int yAmount;
    public int spacing;
    
    // Start is called before the first frame update
    void Start()
    {
        for (int i = -xAmount/2; i < xAmount/2 ; i++)
        {
            for (int j = -xAmount/2; j < yAmount/2; j++)
            {
                Instantiate(yourPrefab,transform.position + new Vector3(i* spacing,0, j*spacing),Quaternion.identity);
            }
        }
    }

   
}
